import { Typography } from 'antd';
import { DownloadOutlined } from '@ant-design/icons';
import axios from "axios";
import fileDownload from 'js-file-download';
import teaser from "../Teaser.png";
const { Title } = Typography;

const StepOne = ({nextStep})=>{

    const downloadPackage = async(fileName)=>{
        try {
            const response = await axios.get(`https://140.112.179.142/packages/${fileName}`, {responseType: "blob"});
            const { data } = response;
            fileDownload(data, fileName);
          } catch (error) {
            console.error(error);
          }
    };

    const OnDownload = ()=>{
        ["HC_base.unitypackage", "HC_prop_editor.unitypackage"].forEach((fileName)=>{downloadPackage(fileName)});
        nextStep();
    }

    return         <div className="block">
    <div style={{ backgroundImage: `url(${teaser})`, width: "100%", minHeight: "360px", backgroundRepeat: "no-repeat", backgroundSize: "cover", opacity: ".7" }} />
    <Title style={{ color: "#e85d04"}}>HaptiCenter Device Developer Workflow</Title>
    <Title level={5} style={{fontStyle: "italic"}}>Register your haptic devices on HaptiCenter</Title>
    <Title level={4} style={{ paddingTop: "70px" }}>Download and install the HC base package and the HC Prop editor</Title>
    <button style={{ border: ".5px dashed", borderRadius: "50%", backgroundColor: "#fff", padding: "10px", cursor: "pointer", color: "#1890ff" }} onClick={OnDownload}>
      <DownloadOutlined style={{ fontSize: "40px"}} />
    </button>
  </div>;
};

export default StepOne;