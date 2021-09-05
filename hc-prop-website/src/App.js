import 'antd/dist/antd.css';
import "./App.css"
import { Layout, Menu } from 'antd';
import logo from "./HCLogo.svg";
import { Steps } from 'antd';
import StepOne from './StepOne';
import StepTwo from './StepTwo';
import StepThree from './StepThree';
import StepFour from './StepFour';


import { useState } from 'react';

const { Header, Content, Footer } = Layout;

const { Step } = Steps;

const StepContent = (step, nextStep)=>{
  return (step === 0)? <StepOne nextStep={nextStep}/>: (step === 1)? <StepTwo nextStep={nextStep}/>: (step === 2)? <StepThree nextStep={nextStep}/>: <StepFour/>;
};

function App() {
  const [step, setStep] = useState(0);
  const nextStep = ()=>{ setStep(step + 1)};


  return (
    <Layout className="layout">
      <img src={logo} className="logo" alt="logo"/>
      <Header className="header">
        <Menu mode="horizontal" defaultSelectedKeys={['0']} style={{ flexDirection: "row-reverse" }}>
          <Menu.Item key={0}>Quick Start</Menu.Item>
          <Menu.Item key={1}>Doc</Menu.Item>
        </Menu>
      </Header>
      <Content style={{ backgroundColor: "#fff" }}>
        {
          StepContent(step, nextStep)
        }

      </Content>
      <Footer style={{backgroundColor: "#fff"}}>  
        <Steps current={step}>
          <Step onStepClick={()=>{setStep(0);}} title="Install" description="Download and install the HC base Unity package" />
          <Step onStepClick={()=>{setStep(1);}} title="Setup" description="Drag Assets/Prefabs/Util.prefab to your scene." />
          <Step onStepClick={()=>{setStep(2);}} title="Explore" description="Select devices that fits your game" />
          <Step onStepClick={()=>{setStep(3);}} title="Use" description="Install and use the device's package" />
        </Steps>
      </Footer>
    </Layout>
  );
}

export default App;
