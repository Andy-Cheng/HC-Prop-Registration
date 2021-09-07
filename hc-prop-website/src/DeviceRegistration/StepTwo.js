import { Typography, Button } from 'antd';
import fillGif from "../FillDeviceInfo.gif"
import { RightOutlined } from '@ant-design/icons';


const { Title } = Typography;

const StepTwo = ({nextStep})=>{

    return         <div className="block">
        <img src={fillGif} alt="Fill info of your device" style={{width: "70%", maxWidth: "960px"}}/>
        <Button onClick={nextStep} size="large" type="primary" icon={<RightOutlined/>} style={{display: "block", marginTop: "16px", marginLeft: "50%", transform: "translate(-60%, 0)"}}>Next Step</Button>
  </div>;
};

export default StepTwo;