// This code is for communicating from Shifty NodeMCU to Unity Server

#include <ESP8266WiFi.h>
#include <WiFiUdp.h>

String deviceName = "Shifty";
int myID;

/* WIFI */
const char *ssid =  "andy";     // replace with your wifi ssid and wpa2 key
const char *pass =  "cheng1134";
const char *host = "192.168.3.194";
int port = 26950;
WiFiClient client;
WiFiUDP udpClient;

/* Command */
struct WelcomeBack{
  int packetLength = 52;
  int cmdID = 1;
  int deviceType;
  int len = 40;
  byte msg[40];
};
struct ShiftyStatus{
  int packetLength = 12;
  int cmdID = 41;
  int functionID = 1;
  int currentMode;
};

/*Stepper Motor Setup*/
int stepPin  = 4; //D2
int dirPin = 5; //D1
int number_of_steps = 200; //total number of steps for this motor
int motorSpeed = 200;
int last_step_time = 0;
unsigned long step_delay = 60L*1000L /number_of_steps/motorSpeed;

int Shifty_Mode = 0; //0: Tennis, 1: Sword
int motorDistance = 650;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(500000);
  Setup_WIFI();
  Server_Connect();
  Setup_Motor();
}

void loop() {
//  if(!client.connected()){
//    Server_Connect();
//    CleanSerial();
//  }
  if (client.connected() && client.available()){
     ReadCommandFromServer();
  }

}

void ReadCommandFromServer(){  
  byte packetLengthBytes[4];
  for (int i = 0; i < 4;i++){
    packetLengthBytes[i] = client.read();
  }
  int packetLength = ByteToInt(packetLengthBytes);
  byte command[packetLength];
  for (int i = 0; i < packetLength; i++){
    command[i] = client.read();
  }
  int cmdID = ByteToInt(command,0,4);
  switch(cmdID){
    case 1:
      myID = ByteToInt(command, 4, 8);
      Serial.println("My id is: " + String(myID));
      SendWelcomeBack();
      break;
    case 2:
      InitializeUDP();
      break;
    case 3:
      OnPacket();
      break;
    case 43:
      int dir = ByteToInt(command, 4, 8);
      int motorStep = ByteToInt(command, 8, 12);
      Move(dir, motorStep);
      break;
  }
}


void SendWelcomeBack(){
  WelcomeBack* packet = new WelcomeBack();
  packet->deviceType = 0;
  deviceName.getBytes(packet->msg, deviceName.length()+1);
  client.write((byte*)packet, sizeof(*packet));
}

void InitializeUDP(){
    Serial.println("local port: " + String(client.localPort()) + "remote port: " + String(client.remotePort()) );
    udpClient.begin(client.localPort());
    udpClient.beginPacket(host, port);
    byte* id = new byte[4];
    intToByte(myID, id);
    int sent_id = ByteToInt(id);
    Serial.println("sent id: " + String(sent_id));
    udpClient.write(id, 4);
    udpClient.endPacket();
}

void OnPacket()
{
  byte functionID_bytes[4];
  for (int i = 0; i < 4; ++i){
    functionID_bytes[i] = client.read();
  }
  int functionID = ByteToInt(functionID_bytes);
  HandleFunctions(functionID);
}

void HandleFunctions(int functionID)
{
  if(functionID == 0)
  {
    ReportStatus();
  }
  else if(functionID == 1)
  {
    byte mode_bytes[4];
    for (int i = 0; i < 4; ++i){
      mode_bytes[i] = client.read();
    }
    int next_mode = ByteToInt(mode_bytes);
    SwitchMode(next_mode);
  }
  
}

void SwitchMode(int next_mode){
  if (next_mode == 0){
    TennisMode();
    Shifty_Mode = 0;
    Serial.println("Shifty changes state to tennis");
  }
  else if (next_mode == 1){
    SwordMode();
    Shifty_Mode = 1;
    Serial.println("Shifty changes state to sword");
  }
  ReportStatus();
}
void ReportStatus(){
  ShiftyStatus* packet = new ShiftyStatus();
  packet->currentMode = Shifty_Mode;
  client.write((byte*)packet, sizeof(*packet));
}
void SetMode(int next_mode){
  Shifty_Mode = next_mode;
}

void Move(int dir, int distance){
  digitalWrite(dirPin, dir);
  for (int i =0; i <= distance; i++){
    digitalWrite(stepPin, HIGH);
    delay(3);
    digitalWrite(stepPin, LOW);
    delay(3);
  }
  Serial.println("Move");
}


void TennisMode(){
  if(Shifty_Mode!= 0){
    Move(1, motorDistance);
  }
}
void SwordMode(){
  if(Shifty_Mode != 1){
    Move(0, motorDistance);
  }
}

void Setup_Motor(){
  pinMode(stepPin, OUTPUT);
  pinMode(dirPin, OUTPUT);
  digitalWrite(stepPin, LOW);
  digitalWrite(dirPin, LOW);  
}

// Connection 
void Setup_WIFI(){
  WiFi.begin(ssid, pass); 
  while (WiFi.status() != WL_CONNECTED) 
  {
    delay(500);
  }
}
void Server_Connect(){
  while(!client){
    client.connect(host, port);
    delay(500);
  }
}
void CleanSerial(){
  while(Serial.read() != -1){}
}
