import 'antd/dist/antd.css';
import "./App.css"
import { Layout, Menu } from 'antd';
import logo from "./HCLogo.svg";
import { Steps } from 'antd';
import StepOne from './StepOne';
import StepTwo from './StepTwo';
import StepThree from './StepThree';
import StepFour from './StepFour';
import RegisterDeviceSteps from './RgisterDeviceSteps';
import Doc from "./Doc";
import {
  Switch,
  Route,
  Link,
  useLocation
} from "react-router-dom";

import { useState } from 'react';

const { Header, Content, Footer } = Layout;

const { Step } = Steps;

const StepContent = ({setSelectedKey})=>{
  const [step, setStep] = useState(0);
  const nextStep = ()=>{ setStep(step + 1)};


  return <>
  {
  (step === 0)? <StepOne nextStep={nextStep}/>: (step === 1)? <StepTwo nextStep={nextStep}/>: (step === 2)? <StepThree nextStep={nextStep}/>: <StepFour setSelectedKey={setSelectedKey}/>
  }
        <Footer style={{backgroundColor: "#fff"}}>  
        <Steps current={step}>
          <Step onStepClick={()=>{setStep(0);}} title="Install" description="Download and install the HC base Unity package." />
          <Step onStepClick={()=>{setStep(1);}} title="Setup" description="Drag Assets/Prefabs/Util.prefab to your scene." />
          <Step onStepClick={()=>{setStep(2);}} title="Explore" description="Select devices that fits your game." />
          <Step onStepClick={()=>{setStep(3);}} title="Use" description="Install and use the device's package." />
        </Steps>
      </Footer>
  </>
};



function App() {
  const location = useLocation();
  const initalKey  = location.pathname === "/"? "0": location.pathname === "/register-device"? "1": "2";
  const [selectedKey, setSelectedKey] = useState(initalKey);

  return (
    <Layout className="layout">
      <img src={logo} className="logo" alt="logo"/>
      <Header className="header">
        <Menu mode="horizontal" selectedKeys={[selectedKey]} style={{ flexDirection: "row-reverse" }}>
          <Menu.Item key={0}><Link onClick={()=>{setSelectedKey("0");}} to="/">Add Haptics To Your Game</Link></Menu.Item>
          <Menu.Item key={1}><Link onClick={()=>{setSelectedKey("1");}} to="/register-device">Register Haptic Devices On HC</Link></Menu.Item>
          <Menu.Item key={2}><Link onClick={()=>{setSelectedKey("2");}} to="/doc">Devices API</Link></Menu.Item>
        </Menu>
      </Header>
      <Content style={{ backgroundColor: "#fff" }}>
        <Switch>
          <Route path="/doc">
            <Doc/>
          </Route>
          <Route path="/register-device">
            <RegisterDeviceSteps setSelectedKey={setSelectedKey}/>
          </Route>
          <Route path="/">
            <StepContent setSelectedKey={setSelectedKey}/>
          </Route>
        </Switch>
      </Content>
    </Layout>
  );
}

export default App;
