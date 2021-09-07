import { Typography, Button } from 'antd';
import uploadGIF from "../UploadDevice.gif"
import { RightOutlined } from '@ant-design/icons';
import { useHistory } from "react-router-dom";


const { Title } = Typography;

const StepThree = ({setSelectedKey})=>{
    let history = useHistory();

    return         <div className="block">
        <img src={uploadGIF} alt="Drag Util to your scene" style={{width: "70%", maxWidth: "960px"}}/>
        <Button onClick={()=>{setSelectedKey("2"); history.push("/doc");}} size="large" type="primary" icon={<RightOutlined/>} style={{display: "block", marginTop: "16px", marginLeft: "50%", transform: "translate(-60%, 0)"}}>Check the device's API</Button>
  </div>;
};

export default StepThree;