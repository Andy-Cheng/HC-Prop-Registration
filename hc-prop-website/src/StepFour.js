import { Typography, Button } from 'antd';
import devicePackageGIF from "./device_package.gif"
import { useHistory } from "react-router-dom";


const { Title } = Typography;

const StepFour = ({setSelectedKey})=>{
  let history = useHistory();

    return         <div className="block">
        <img src={devicePackageGIF} alt="Drag Util to your scene" style={{width: "70%", maxWidth: "900px"}}/>
        <Title level={2}>Design your prop representing the devivce and start using the device in your game!</Title>
        <Title level={3}>Need help?</Title>
        <Button onClick={()=>{setSelectedKey("2"); history.push("/doc");}} size="large" type="primary"  style={{display: "block", marginTop: "16px", marginLeft: "50%", transform: "translate(-50%, 0)"}}>Reference the document</Button>
  </div>;
};

export default StepFour;