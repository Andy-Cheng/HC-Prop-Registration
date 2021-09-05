import { Typography, Button } from 'antd';
import dragGIF from "./drag.gif"


const { Title } = Typography;

const StepTwo = ()=>{

    return         <div className="block">
        <img src={dragGIF} alt="Drag Util to your scene" style={{width: "70%", maxWidth: "960px"}}/>
        <Title level={2}>Start using the device in your game!</Title>
        <Title level={3}>Need help?</Title>
        <Button size="large" type="primary"  style={{display: "block", marginTop: "16px", marginLeft: "50%", transform: "translate(-50%, 0)"}}>Reference the document</Button>
  </div>;
};

export default StepTwo;