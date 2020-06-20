# Swifter.MessagePack
#### A powerful, easy-to-use and fastest msgpack serializer and deserializer on .Net platforms.

### If you want to use Swifter.MessagePack, please download or install the latest version on [Nuget](https://www.nuget.org/packages/Swifter.MessagePack).
### 如果您想使用 Swifter.MessagePack，请在 [Nuget](https://www.nuget.org/packages/Swifter.MessagePack) 上下载或安装最新版本。

### Easy to use 简单使用
```C#
public class Demo
{
    public int Id { get; set; }
    public int Name { get; set; }
    public static void Main()
    {
        byte[] msgpack = MessagePackFormatter.SerializeObject(new { Id = 123, Name = "Dogwei" });
        var dic = MessagePackFormatter.DeserializeObject<Dictionary<string, object>>(msgpack);
        var obj = MessagePackFormatter.DeserializeObject<Demo>(msgpack);
    }
}
```


## Supported data structures and types 支持的数据类型和结构
```C#
bool, byte, sbyte, short, ushort, char, int, uint, long, ulong, IntPtr, UIntPtr,
float, double, decimal, string, enum, DateTime, DateTimeOffset, TimeSpan, Guid,
BigInteger, Complex, DBNull, Nullable<T>, Tuple<...>, ValueTuple<...>, Version,
Uri, Assembly, Type, MemberInfo, MethodInfo, FieldInfo, PropertyInfo, ConstructorInfo,	
EventInfo, Array, Multidimensional-Arrays, IList, IList<T>, ICollection, ICollection<T>,	
IDictionary, IDictionary<TKey, TValue>, IEnumerable, IEnumerable<T>, DataSet, DataTable,	
DataRow, DbRowObject, DbDataReader...	
Other types are treated as object 其他类型当作对象处理	
```	

## Supported platforms and runtimes 支持的平台和运行时
```
.NET Framework 2.0+, .NET Core 2.0+, .NET Standard 2.0+, MONO, MONO AOT, MONO FULL-AOT,
Unity, Xamarin.iOS, Xamarin.Android

Uncertain：Unity IL2CPP

Unsupported: Sliverlight

Note:
    .NET Core use the Core version, and other platforms use the Framework version or Standard version.
    Because the Core version is performance-optimized.
    the Framework version and Standard version are optimized for compatibility.
    the Framework version and Standard version can run directly on AOT platforms.
注意：
    .NET Core 请使用 Core 版本，其他平台和运行时请使用 Framework 版本或 Standard 版本。
    因为 Core 版本专为性能优化，Framework 版本和 Standard 版本为兼容性优化。
    Framework 版本和 Standard 版本可以直接在 AOT 平台上运行。
```

## Supported features 支持的功能
```
LoopReferencingNull:
    Objects that appear a loop reference during serialization are treated as Null.
    在序列化时出现循环引用的对象将用 Null 表示。
    
MultiReferencingReference:
    Allow use { "$ref": "#/obj/1/target" } to represent objects that are repeatedly referenced.
    允许使用 { "$ref": "#/obj/1/target" } 写法表示重复引用的对象。
    
AsOrderedObjectDeserialize
    Perform as-ordered object fields parsing, which can improve parsing performance of ordered msgpack objects.
    执行假定有序的对象字段解析，这可以提高有序 msgpack 对象的解析性能。
    
CamelCaseWhenSerialize
    Convert the fields name in the object to camel case during serialization. 
    序列化时，将对象中的字段名称转换为骆驼命名法。 ::: new { Name = "Dogwei" } -> { "name": "Dogwei" }
    
IgnoreNull | IgnoreZero | IgnoreEmptyString
    Null, 0, "" values are ignored during serialization. 
    序列化时分别跳过 Null, 0 和 "" 值。 ::: { A = (string)null, B = 0, C = "", D = 1 } -> { "D": 1 }
    
For more features, please see Swifter.MessagePack.MessagePackFormatterOptions enum.
更多功能请看 Swifter.MessagePack.MessagePackFormatterOptions 配置项。
```
## Performance 性能
![Performance](performance.png)

## Demos 示例
##### (1) Deserialize to dynamic 反序列化为 dynamic
```C#
        var list = new List<object>
        {
            { new Dictionary<string, object>() { { "Id", 1 }, { "Name", "Dogwei" } }},
            { new Dictionary<string, object>() { { "Id", 2 }, { "Name", "sg" } }},
            { new Dictionary<string, object>() { { "Id", 3 }, { "Name", "cxw" } }},
            { new Dictionary<string, object>() { { "Id", 4 }, { "Name", "eway" } }},
            {
                new Dictionary<string, object>() { 
                    { "Id", 5 }, 
                    { "Name", "Xinwei Chen" }, 
                    { "Data", new Dictionary<string, object> { { "Age", 21 }, { "Sex", "Male" } } }
                }
            },
        };

        var msgpack = MessagePackFormatter.SerializeObject(list);

        dynamic dym = MessagePackFormatter.DeserializeObject<MessagePackValue>(msgpack);

        Console.WriteLine(dym[0].Name); // Dogwei
        Console.WriteLine(dym[1].Name); // sg
        Console.WriteLine(dym[2].Id); // 3
        Console.WriteLine(dym[3].Id); // 4
        Console.WriteLine(dym[4].Data.Age); // 21
```
##### (2) Attributes 特性
```C#
[RWObject(SkipDefaultValue = RWBoolean.Yes)]
public class Demo
{
    public int Id;
    public string Name;
    [RWField("Age")]
    private int age;
    [RWField(SkipDefaultValue = RWBoolean.No)]
    public int? Sex;
    [RWFormat("yyyy-MM-dd")]
    public DateTime Birthday { get; set; }

    public static void Main()
    {
        var obj = new Demo { Name = "Dogwei", age = 24, Birthday = DateTime.Parse("1996-01-08") };
        var msgpack = MessagePackFormatter.SerializeObject(obj);
        var dest = MessagePackFormatter.DeserializeObject<Demo>(msgpack);

        Console.WriteLine(msgpack.ToHexString());
        // 84a341676518a84269727468646179aa313939362d30312d3038a44e616d65a6446f67776569a3536578c0
        // as Json: {"Age":24,"Birthday":"1996-01-08","Name":"Dogwei","Sex":null}
    }
}
```
##### (3) Advanced 进阶用法
```C#
    var datatable = ValueCopyer.ValueOf(new[] {
        new { Id = 1, Guid = Guid.NewGuid(), Name = "Dogwei" },
        new { Id = 2, Guid = Guid.NewGuid(), Name = "cxw" },
        new { Id = 3, Guid = Guid.NewGuid(), Name = "sg" },
        new { Id = 4, Guid = Guid.NewGuid(), Name = "eway" },
    }).ReadDataTable();
    
    var msgpackFormatter = new MessagePackFormatter();
    msgpackFormatter.SetDataTableRWOptions(DataTableRWOptions.WriteToArrayFromBeginningSecondRows);
    msgpackFormatter.SetValueFormat<Guid>("N");

    var msgpack = msgpackFormatter.Serialize(datatable);
    var dest = MessagePackFormatter.DeserializeObject<DataTable>(msgpack);

    Console.WriteLine(msgpack.ToHexString());
    /*
    9483a447756964d9203466383835386665303961363466323962643436653930363433373037343630a2496401a44e616d65a6446f6777656993d920343862333466643466343265343363313839643066343933623963366363623902a363787793d920353334323139613332383865346630306234663463306661626130653533313103a2736793d920363462663635343463373436346464653836366138386366626235376436313904a465776179
    */
    /* as Json:
    [
      {
        "Guid": "1615527f673c499fac8de16847ad8783",
        "Id": 1,
        "Name": "Dogwei"
      },
      [
        "a23d1980185749118796fb5db7fb57a1",
        2,
        "cxw"
      ],
      [
        "9f76a802148d420da52716cf8a90b13d",
        3,
        "sg"
      ],
      [
        "ba03739cd44a49fab7b3de2558f84ebe",
        4,
        "eway"
      ]
    ]
    */
```
```C#
    var dic = new Dictionary<string, object>
    {
        { "Id", 123 },
        { "SystemNo", "9110" },
        { "IMEI", 31415926535897UL }
    };
    
    var msgpackFormatter = new MessagePackFormatter();
    msgpackFormatter.SetValueInterface(new MyUInt64Interface());

    var msgpack = msgpackFormatter.Serialize(dic);
    var obj = new { Id = 0, SystemNo = "", IMEI = 0UL, SIMId = 999 };

    msgpackFormatter.DeserializeTo(msgpack, RWHelper.CreateWriter(obj));

    Console.WriteLine(msgpack);
    // 83a249647ba853797374656d4e6fa439313130a4494d4549ae3078314339323937323433364439
    // as Json: {"Id":123,"SystemNo":"9110","IMEI":"0x1C92972436D9"}

    Console.WriteLine(obj.Id); // 123
    Console.WriteLine(obj.IMEI); // 31415926535897
    Console.WriteLine(obj.SIMId); // 999

    public class MyUInt64Interface : IValueInterface<ulong>
    {
        public unsafe ulong ReadValue(IValueReader valueReader)
        {
            var str = valueReader.ReadString();

            fixed (char* chars = str)
            {
                return NumberHelper.GetNumberInfo(chars, str.Length).ToUInt64(16);
            }
        }

        public void WriteValue(IValueWriter valueWriter, ulong value)
        {
            valueWriter.WriteString($"0x{value:X}");
        }
    }
```
