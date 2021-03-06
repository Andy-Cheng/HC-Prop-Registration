import { Typography } from 'antd';
import { DownloadOutlined } from '@ant-design/icons';

import teaser from "./Teaser.png";
const { Title } = Typography;

const StepOne = ({nextStep})=>{

    const OnDownload = ()=>{
        nextStep();
    }

    return         <div className="block">
    <div style={{ backgroundImage: `url(${teaser})`, width: "100%", minHeight: "360px", backgroundRepeat: "no-repeat", backgroundSize: "cover", opacity: ".7" }} />
    <Title style={{ color: "#e85d04"}}>HaptiCenter Game Developer Workflow</Title>
    <Title level={5} style={{fontStyle: "italic"}}>Explore and integrate haptic devices to your games</Title>
    <Title level={4} style={{ paddingTop: "70px" }}>Download and install the HC base package</Title>
    <a href="https://140.112.179.142/packages/HC_base.unitypackage" download>
    <button style={{ border: ".5px dashed", borderRadius: "50%", backgroundColor: "#fff", padding: "10px", cursor: "pointer" }} onClick={OnDownload}>
      <DownloadOutlined style={{ fontSize: "40px" }} />
    </button>
    </a>
  </div>;
};

export default StepOne;