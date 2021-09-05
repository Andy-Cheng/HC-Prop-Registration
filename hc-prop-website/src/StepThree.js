import { Typography } from 'antd';
import Doc from "./Doc";

const { Title } = Typography;

const StepThree = ({nextStep})=>{ 
    return <Doc nextStep={nextStep}/>;
};

export default StepThree;