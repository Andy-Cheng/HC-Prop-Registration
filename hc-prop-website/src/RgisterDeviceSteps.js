import { Layout, Steps } from 'antd';
import { useState } from 'react';
import StepOne from './DeviceRegistration/StepOne';
import StepTwo from './DeviceRegistration/StepTwo';
import StepThree from './DeviceRegistration/StepThree';

const { Footer } = Layout;

const { Step } = Steps;

const RegisterDeviceSteps = ({setSelectedKey})=>{
    const [step, setStep] = useState(0);
    const nextStep = ()=>{ setStep(step + 1)};
  
    return <>
    {
    (step === 0)? <StepOne nextStep={nextStep}/>: (step === 1)? <StepTwo nextStep={nextStep}/>: <StepThree setSelectedKey={setSelectedKey}/>
    }
          <Footer style={{backgroundColor: "#fff"}}>  
          <Steps current={step}>
            <Step onStepClick={()=>{setStep(0);}} title="Install" description="Download and install the HC base Unity package and editor." />
            <Step onStepClick={()=>{setStep(1);}} title="Edit" description="In Unity, open window/HC Prop Creator. Fill in the device's information." />
            <Step onStepClick={()=>{setStep(2);}} title="Upload" description="Upload the devivce's information and Unity package." />
          </Steps>
        </Footer>
    </>
  };

  export default RegisterDeviceSteps;