int ByteToInt(byte* byteArray){
  int value = 0;
  int offset = 8;
  for (int i = 0; i < sizeof(byteArray); i++){
      value = ((int)byteArray[i]) << (offset*i) | value;
  }
    return value;
}

int ByteToInt(byte* byteArray, int _start, int _stop){
  int value = 0;
  int offset = 8;
  for (int i = _start; i < _stop; i++){
      value = ((int)byteArray[i]) << (offset*(i-_start)) | value;
  }
    return value;
}

void intToByte(int num, byte* byteArray){
  byteArray[0] = num & 0x000000ff;
  byteArray[1] = num & 0x0000ff00;
  byteArray[2] = num & 0x00ff0000;
  byteArray[3] = num & 0xff000000;
  
  
}


String ByteToString(byte* byteArray, int _start, int _stop){
  String msg = "";
  for (int i = _start; i < _stop; i++){
    msg += (char)byteArray[i];
  }
  return msg;
}
