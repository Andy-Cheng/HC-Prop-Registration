import axios from "axios";
import { useState, useEffect } from "react";
import { Typography, Table, Checkbox, Tag, Collapse } from "antd";
import { useLocation } from "react-router-dom";

const { Title, Text, Paragraph } = Typography;
const { Panel } = Collapse;

const ParamTypes = ["short", "int", "long", "float", "bool", "string", "Vector3", "Quaternion"];
const CommuOptions = ["Network", "Bluetooth"];
const TrackingOptions = ["VIVE Tracker"];

const transformMessage = {
    functionName: "RecieveTransform",
    functionDescription: "Recieve the Transform of the tracker mounted on the device.",
    parameters: [{name: "trackerID", type: 1, description: "The ID of the tracker mounted on the device."}, {name: "rotation", type: 7, description: "A Quaternion that stores the rotation of tracker's the Transform in world space."}, {name: "position", type: 6, description: "The world space position of the tracker's Transform."}],
};

const Parameters = ({paramInfo})=>{
    const dataSource = paramInfo.map((parameter, index)=>{
        return {
            key: index,
            name: parameter.name,
            type: ParamTypes[parameter.type],
            description: parameter.description
        }
    });
      
      const columns = [
        {
          title: 'Name',
          dataIndex: 'name',
        },
        {
          title: 'Type',
          dataIndex: 'type',
        },
        {
          title: 'Description',
          dataIndex: 'description',
        },
      ];
      
    return <Table pagination={false} dataSource={dataSource} columns={columns} />;
}

// type: 0: function, 1: message
const Function = ({functionInfo, type})=>{
    const buildParamString = (accString, currentParam, currentIndex, array)=>{
        if(currentIndex > 0)
        {
            accString += ", ";
        }
        accString += `${ParamTypes[currentParam.type]} ${currentParam.name}`;
        return accString;
    };
    let paramStringStart;
    let paramStringEnd;
    if(type === 0)
    {
        paramStringStart = "(";
        paramStringEnd = ");";
    }
    else
    {
        paramStringStart = "<";
        paramStringEnd = ">";
    }
    let paramString = functionInfo.parameters.reduce(buildParamString, paramStringStart) + paramStringEnd;
    let declration = (type === 0)? `public void ${functionInfo.functionName}${paramString}`: `public event Action${paramString} On${functionInfo.functionName}`;
    

    return <div>
        <Title level={4}>{declration}</Title>        
        <Title level={5}>Description</Title>
        <Paragraph>{functionInfo.functionDescription}</Paragraph>
        {
            functionInfo.parameters.length > 0 ? <><Title level={5}>Parameters</Title>
            <Parameters paramInfo={functionInfo.parameters}/></> : null
        }
    </div>
}






const Device = ({handleSelection, deviceInfo, isDOc})=>{
    const OnChangeCheckBox = (e)=>{
        handleSelection(e.target.checked, deviceInfo.name);
    }

    return <>
        <Title style={{display: "inline-block"}} level={2}>{deviceInfo.name}</Title>
        {
            !isDOc && <><Text strong style={{display: "inline-block", marginLeft: "32px"}}>select this device</Text> <Checkbox onChange={OnChangeCheckBox}/></>
        }
        <br/>
        <Title style={{display: "inline-block"}} level={5}>Communication Method: </Title> 
        <Tag color="orange" style={{marginLeft: "8px"}}>{CommuOptions[deviceInfo.commuMethod]}</Tag>
        <Title style={{display: "inline-block"}} level={5}>Tracking Method: </Title> 
        <Tag color="blue" style={{marginLeft: "8px"}}>{TrackingOptions[deviceInfo.trackingMethod]}</Tag>
        <Title level={4}>Description</Title>
        <Paragraph>{deviceInfo.description}</Paragraph>

        <Title level={4} style={{lineHeight: "0px"}}>Constructor </Title> 
        <Title level={4} >{`public ${deviceInfo.name}();`}</Title>        

        <Collapse defaultActiveKey={["0", "1"]}>
        {/* <Title level={4}> </Title>  */}
        <Panel forceRender key="0" header="Functions to control the device (Game -> Device)">
        {
            deviceInfo.functions.map((functionInfo, index)=><Function type={0} functionInfo={functionInfo} key={index}/>)
        }
        </Panel>
        {/* <Title level={4}>Messages (Device to Game) </Title>  */}
        <Panel forceRender key="1" header="Messages sent from the device to games(Device -> Game)">
        {
            [transformMessage, ...deviceInfo.messages].map((functionInfo, index)=><Function type={1} functionInfo={functionInfo} key={index}/>)
        }
        </Panel>
        </Collapse>

    </>
}




const Doc = ({handleSelection})=>{
    const location = useLocation();
    const isDoc = location.pathname === "/doc";
    const getDevices = async ()=>{
        try {
            const response = await axios.get("https://140.112.179.142/devices");
            const { data } = response;
            setDevices(data);
          } catch (error) {
            console.error(error);
          }
    };

    const [devices, setDevices] = useState([]);


    useEffect(()=>{
        getDevices();
    }, []);



    return <>

    <div className="doc" style={isDoc? {}:{height: "calc(100vh - 232px)", overflowY: "scroll"}}>
        {
            devices.map((deviceInfo, index)=><Device handleSelection={handleSelection} deviceInfo={deviceInfo} isDOc={isDoc} key={index}/>)
        }
    </div>

    </>;
};


export default Doc;