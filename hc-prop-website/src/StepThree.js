import { useState, useEffect } from "react";
import { Typography, Button, Tag } from "antd";
import { RightOutlined } from '@ant-design/icons';
import axios from "axios";
import fileDownload from 'js-file-download';
import Doc from "./Doc";


const { Text } = Typography;

const StepThree = ({nextStep})=>{ 
    const [selectedDevices, setSelectedDevices] = useState([]);

    const handleSelection = (checked, deviceName)=>{
        const index = selectedDevices.findIndex((name)=>name === deviceName);
        if(checked)
        {
            setSelectedDevices([...selectedDevices, deviceName]);
        }
        else
        {
            let newDevices = [...selectedDevices];
            newDevices.splice(index, 1);
            setSelectedDevices(newDevices);
        }
    }

    const downloadPackage = async(deviceName)=>{
        const fileName = `${deviceName}.unitypackage`;
        try {
            const response = await axios.get(`https://140.112.179.142/packages/${fileName}`, {responseType: "blob"});
            const { data } = response;
            fileDownload(data, fileName);
          } catch (error) {
            console.error(error);
          }
    };

    const onNextStep = ()=>{
        selectedDevices.forEach(deviceName => {
            downloadPackage(deviceName);
        });
        nextStep();
    }
    return <>
    <Doc handleSelection={handleSelection}/>
    <div style={{ position: "absolute", top: "63px", backgroundColor: "#fff", width: "100%", minHeight: "40px", padding: "16px 48px" }}>
            <Text style={{ display: "inline-block" }} strong>Selected Devices: </Text>
            {
                selectedDevices.map((name, index)=><Tag style={{marginLeft: "8px"}} key={index}>{name}</Tag>)
            }
            <Button onClick={onNextStep} style={{position: "absolute", right: "66px"}}  size="small" type="primary" icon={<RightOutlined/>}>Confirm Selection</Button>
        </div>
    </>;
};

export default StepThree;