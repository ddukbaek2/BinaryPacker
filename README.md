# BinaryPacker

클래스 인스턴스의 공개된 필드 멤버를 바이너리 데이터로 만들거나

바이너리 데이터를 다시 공개된 필드 멤버를 채워서 클래스 인스턴스로 변환해주는 도구입니다.


XML, JSON, BinaryFormatter를 사용하지 않고 순수 Reflection만을 이용해서 만들었습니다.


객체는 Class만을 지원하고 ValueTuple, KeyValuePair 등의 Struct 계열은 지원하지 않습니다.

List와 배열을 지원하고 Dictionary 등의 연관배열은 지원하지 않습니다.

일반적인 변수는 JSON 처럼 Boolean, Number, String 을 사용가능하며 Enum을 추가 타입으로 지원합니다.

Number의 경우 short, int, long, ushort, uint, ulong, float, double을 지원합니다.


내부적으로 오브젝트는 BPObject 라는 자체 객체로 변환되며 byte 변환은 BPObject를 byte로 변환하는 것이므로 BPObject의 내부 구조가 바뀌지 않으면 데이터는 계속 호환됩니다.
