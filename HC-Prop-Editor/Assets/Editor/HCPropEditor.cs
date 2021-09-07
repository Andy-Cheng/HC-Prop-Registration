using UnityEngine;
using UnityEditor;
using Unity.EditorCoroutines.Editor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine.Networking;

public class DeviceDoc{
    public string name;
    public string description;
    public int commuMethod;
    public int trackingMethod;
    public List<Function> functions;
    public List<Function> messages;
}

[Serializable]
public class Parameter{
    public int type;
    public string name;
    public string description;
}

[Serializable]
public class Function{
    public string functionName;
    public string functionDescription;
    public List<Parameter> parameters;

    public Function(){
        parameters = new List<Parameter>();
    }

}


public class WebRequestCert : UnityEngine.Networking.CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}

public class HCPropEditor : EditorWindow
    
    {
    string serverURL = "https://140.112.179.142";
    public string deviceName;
    public string deviceDescription;
    string[] commuOptions = new string[] {"Network", "Bluetooth"};
    string[] trackingOptions = new string[] {"VIVE Tracker"};
    string[] parameterOptions = new string[] {"short", "int", "long", "float", "bool", "string", "Vector3", "Quaternion"};

    public int communicationMethod = 0;
    public int trackingMethod = 0;
    [SerializeField]
    GameObject deviceModel;
    Editor scannedModelEditor;
    Vector2 scrollPosition = Vector2.zero;

    public List<Function> functions = new List<Function>();
    public List<Function> messages = new List<Function>();

    [MenuItem ("Window/HC Prop Creator")]
    public static void  ShowWindow () {
        EditorWindow window = GetWindow(typeof(HCPropEditor));
        window.Show();
    }

    void AddFunction(){
        functions.Add(new Function());
    }

    void AddMessage(){
        messages.Add(new Function());
    }

    GUIStyle block = new GUIStyle();
    GUIStyle smallBlock = new GUIStyle();
    
    
    void RenderParameter(List<Parameter> parameters, int seq){
        smallBlock.padding = new RectOffset(10, 10, 10, 10);
        GUILayout.BeginVertical(smallBlock);
        parameters[seq].name = EditorGUILayout.TextField ("Parameter Name", parameters[seq].name, GUILayout.MaxWidth(400));
        parameters[seq].type = EditorGUILayout.Popup("Type", parameters[seq].type, parameterOptions, GUILayout.MaxWidth(400));
        parameters[seq].description = EditorGUILayout.TextField ("Parameter Description", parameters[seq].description);

        if (GUILayout.Button("Remove Parameter", GUILayout.MaxWidth(400)))
        {
            parameters.RemoveAt(seq);
        }
        GUILayout.EndVertical();
    }

    void RenderFunction(int seq){
        block.padding = new RectOffset(20,20,20,20);
        GUILayout.BeginVertical(block);


        functions[seq].functionName = EditorGUILayout.TextField ("Function Name", functions[seq].functionName, GUILayout.MaxWidth(400));
        functions[seq].functionDescription = EditorGUILayout.TextField ("Function Description", functions[seq].functionDescription);
        for(int i = 0; i < functions[seq].parameters.Count; ++i){
            RenderParameter(functions[seq].parameters, i);
        }
        if (GUILayout.Button("Add Parameter", GUILayout.MaxWidth(400)))
        {
            functions[seq].parameters.Add(new Parameter());
        }
        if (GUILayout.Button("Remove Function"))
        {
            functions.RemoveAt(seq);
        }

        GUILayout.EndVertical();
        
    }

    void RenderFunctions(){
        for(int i = 0; i < functions.Count; ++i){
            RenderFunction(i);
        }
    }

    void RenderMessage(int seq){
        block.padding = new RectOffset(20,20,20,20);
        GUILayout.BeginVertical(block);
        messages[seq].functionName = EditorGUILayout.TextField ("Message Name", messages[seq].functionName, GUILayout.MaxWidth(400));
        messages[seq].functionDescription = EditorGUILayout.TextField ("Message Description", messages[seq].functionDescription);
        for(int i = 0; i < messages[seq].parameters.Count; ++i){
            RenderParameter(messages[seq].parameters, i);
        }
        if (GUILayout.Button("Add Parameter", GUILayout.MaxWidth(400)))
        {
            messages[seq].parameters.Add(new Parameter());
        }
        if (GUILayout.Button("Remove Message"))
        {
            messages.RemoveAt(seq);
        }

        GUILayout.EndVertical();
    }

    void RenderMessages(){
        for(int i = 0; i < messages.Count; ++i){
            RenderMessage(i);
        }
    }


    string Captalize(string word)
    {
        return word.ToUpper().Substring(0, 1) + word.Substring(1);
    }

    void GenerateCode(string assetPath)
    {
        string[] nameSpaces = new string[] {"UnityEngine", "System", "HC.Network"};

        using (StreamWriter outputFile = new StreamWriter(Path.Combine(assetPath, $"{deviceName}.cs")))
        {
            // Include namespaces
            foreach(string nameSpace in nameSpaces)
            {
                outputFile.WriteLine($"using {nameSpace};");
            }
            outputFile.WriteLine("");

            // Device code
            outputFile.WriteLine($"namespace HC.{deviceName}" + "{");
            
            outputFile.WriteLine($"public class {deviceName}" + "{");

            outputFile.WriteLine($"public string deviceName = \"{deviceName}\";");

            outputFile.WriteLine($"public {deviceName}()" + "{");
            outputFile.WriteLine("ClientHandle.DeviceNameToHandle.Add(deviceName, DeviceHandle);");
            outputFile.WriteLine("}");
            outputFile.WriteLine("");

            outputFile.WriteLine("// Device to player");
            outputFile.WriteLine($"/// <summary>Recieve the transform of the tracker with id. int: id, Quaternion: rotation, Vector3: position</summary>");
            outputFile.WriteLine("public event Action<int, Quaternion, Vector3> OnRecieveTransform;");
            for(int i = 0; i < messages.Count; ++i)
            {
                StringBuilder eventStringBuilder = new StringBuilder("public event Action <");
                StringBuilder commentBuilder = new StringBuilder($"/// <summary>{messages[i].functionDescription} ");
                for(int paramSeq = 0; paramSeq < messages[i].parameters.Count; ++paramSeq)
                {
                    if(paramSeq != 0)
                    {
                        eventStringBuilder.Append(", ");
                        commentBuilder.Append(", ");
                    }
                    eventStringBuilder.Append(parameterOptions[messages[i].parameters[paramSeq].type]);
                    commentBuilder.Append($"{parameterOptions[messages[i].parameters[paramSeq].type]}: {messages[i].parameters[paramSeq].name}");
                }
                eventStringBuilder.Append($"> On{messages[i].functionName};");
                commentBuilder.Append("</summary>");
                outputFile.WriteLine(commentBuilder.ToString());
                outputFile.WriteLine(eventStringBuilder.ToString());
            }
            outputFile.WriteLine("");

            outputFile.WriteLine("public void DeviceHandle(Packet _packet){");
            outputFile.WriteLine("int functionID = _packet.ReadInt();");
            outputFile.WriteLine("if(functionID == 0)");
            outputFile.WriteLine("{");
            outputFile.WriteLine("OnRecieveTransform.Invoke(_packet.ReadInt(), _packet.ReadQuaternion(), _packet.ReadVector3());");
            outputFile.WriteLine("}");
            for(int i = 0; i < messages.Count; ++i)
            {
                outputFile.WriteLine($"else if(functionID == {i+1})");
                outputFile.WriteLine("{");
                StringBuilder handleStrBuilder = new StringBuilder("On");
                handleStrBuilder.Append($"{messages[i].functionName}.Invoke(");
                for(int paramSeq = 0; paramSeq < messages[i].parameters.Count; ++paramSeq)
                {
                    if( paramSeq != 0)
                    {
                        handleStrBuilder.Append(", ");
                    }
                    string packet_readType = $"_packet.Read{Captalize(parameterOptions[messages[i].parameters[paramSeq].type])}()";
                    handleStrBuilder.Append(packet_readType);
                }
                handleStrBuilder.Append(");");

                outputFile.WriteLine(handleStrBuilder.ToString());
                outputFile.WriteLine("}");
            }
            outputFile.WriteLine("}");
            outputFile.WriteLine("");

            outputFile.WriteLine("// Player to device");
            for(int funcSeq = 0; funcSeq < functions.Count; ++funcSeq)
            {
                outputFile.WriteLine($"/// <summary>{functions[funcSeq].functionDescription}</summary>");
                StringBuilder funcParamBuilder = new StringBuilder($"public void {functions[funcSeq].functionName}(");
                for(int paramSeq = 0; paramSeq < functions[funcSeq].parameters.Count; ++paramSeq)
                {
                    outputFile.WriteLine($"/// <param name=\"{functions[funcSeq].parameters[paramSeq].name}\">{functions[funcSeq].parameters[paramSeq].description}</param>");
                    if(paramSeq != 0)
                    {
                        funcParamBuilder.Append(", ");
                    }

                    funcParamBuilder.Append($"{parameterOptions[functions[funcSeq].parameters[paramSeq].type]} {functions[funcSeq].parameters[paramSeq].name}");
                }
                funcParamBuilder.Append("){");
                outputFile.WriteLine(funcParamBuilder.ToString());
                outputFile.WriteLine("using(Packet _packet = new Packet()){");
                outputFile.WriteLine("_packet.Write(deviceName); ");
                for (int paramSeq = 0; paramSeq < functions[funcSeq].parameters.Count; ++paramSeq)
                {
                    outputFile.WriteLine($"_packet.Write({functions[funcSeq].parameters[paramSeq].name});");
                }
                outputFile.WriteLine("ClientSend.TransferPacket(_packet);");
                outputFile.WriteLine("}");
                outputFile.WriteLine("}");
            }

            outputFile.WriteLine("}");
            outputFile.WriteLine("}");
        }
    }

    void UploadDevice(){
        string deviceAssetPath = Path.Combine(Application.dataPath, "HC", deviceName);
        try
        {
            // Determine whether the directory exists.
            if (Directory.Exists(deviceAssetPath))
            {
                Debug.Log($"That path {deviceAssetPath} exists already.");
                return;
            }
            // Try to create the directory.
            DirectoryInfo di = Directory.CreateDirectory(deviceAssetPath);
            Debug.Log($"The directory was created successfully at {Directory.GetCreationTime(deviceAssetPath)}.");

        }
        catch (Exception e)
        {
            Debug.Log($"The process failed: {e.ToString()}");
        }

        GenerateCode(deviceAssetPath);
        AssetDatabase.Refresh();
        // Packaged generated code and 3D prefab
        // Save the gameobject as a prefab
        string name = deviceModel.name;
        deviceModel.name= $"{deviceName}_reference_model";
        PrefabUtility.SaveAsPrefabAssetAndConnect(deviceModel, $"Assets/HC/{deviceName}/{deviceName}ReferenceModel.prefab", InteractionMode.AutomatedAction);
        PrefabUtility.UnpackPrefabInstance(deviceModel, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
        deviceModel.name = name;
        string packageName = $"{deviceName}.unitypackage";
        AssetDatabase.ExportPackage($"Assets/HC/{deviceName}", packageName,  ExportPackageOptions.Recurse| ExportPackageOptions.IncludeDependencies);
        AssetDatabase.DeleteAsset($"Assets/HC/{deviceName}");
        AssetDatabase.Refresh();

        // Write to DB
        EditorCoroutineUtility.StartCoroutine(WriteDB(), this);

        // Upload package
        EditorCoroutineUtility.StartCoroutine(UploadPackage($"{Path.GetDirectoryName(Application.dataPath)}/{packageName}", packageName), this);
    }

    IEnumerator WriteDB()
    {
        DeviceDoc doc =  new DeviceDoc();
        doc.name = deviceName;
        doc.description = deviceDescription;
        doc.commuMethod = communicationMethod;
        doc.trackingMethod = trackingMethod;
        doc.functions = functions;
        doc.messages = messages;

        WWWForm form = new WWWForm();
        form.AddField("data", JsonUtility.ToJson(doc));


        using (var w = UnityWebRequest.Post($"{serverURL}/device", form))
        {
            w.certificateHandler = new WebRequestCert();
            yield return w.SendWebRequest();
            if (w.isNetworkError || w.isHttpError) {
                Debug.Log(w.error);
            }
            else {
                Debug.Log($"Finished adding device to DB");
            }
        }

    }

    IEnumerator UploadPackage(string filePath, string fileName)
    {        
        WWWForm form = new WWWForm();
        byte[] packageBytes = File.ReadAllBytes(filePath);
        form.AddBinaryData("package", packageBytes, fileName);
        using (var w = UnityWebRequest.Post($"{serverURL}/package", form))
        {
            w.certificateHandler = new WebRequestCert();
            yield return w.SendWebRequest();
            if (w.isNetworkError || w.isHttpError) {
                Debug.Log(w.error);
            }
            else {
                Debug.Log($"Finished Uploading {fileName}");
                File.Delete(filePath);
            }
        }
    }

    void OnGUI () {
        // The actual window code goes here
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.Label ("Device Base Settings", EditorStyles.boldLabel);
        deviceName = EditorGUILayout.TextField ("Device Name", deviceName, GUILayout.MaxWidth(400));
        deviceDescription = EditorGUILayout.TextField ("Device Description", deviceDescription);
        communicationMethod = EditorGUILayout.Popup("Communication Method", communicationMethod, commuOptions, GUILayout.MaxWidth(400));
        trackingMethod = EditorGUILayout.Popup("Tracking Method", trackingMethod, trackingOptions, GUILayout.MaxWidth(400));
        deviceModel = (GameObject) EditorGUILayout.ObjectField("Device Model", deviceModel, typeof(GameObject), true);
        GUIStyle bgColor = new GUIStyle();
        bgColor.normal.background = EditorGUIUtility.whiteTexture;
        if (deviceModel != null)
        {
            if (scannedModelEditor == null)
                scannedModelEditor = Editor.CreateEditor(deviceModel);

            scannedModelEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(256, 256), bgColor);
        }

        GUILayout.Label ("Functions: Player -> Device", EditorStyles.boldLabel);
        RenderFunctions();
        if (GUILayout.Button("Add Function"))
        {
            AddFunction();
        }
        GUILayout.Space(20);
        GUILayout.Label ("Messages: Device -> Player", EditorStyles.boldLabel);
        RenderMessages();
        if (GUILayout.Button("Add Message"))
        {
            AddMessage();
        }
        GUILayout.Space(40);

        if (GUILayout.Button("Upload Device")){
            UploadDevice();
        }


        GUILayout.EndScrollView();

    }
    }

    