using System.Globalization;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Serialization;

public class StackObject
{
  public enum StackObjectType {Int, Float, String, Bool}

  public StackObjectType Type { get; set; }
  public int Length { get; set; }

  public int Depth { get; set; }

  public string? Id { get; set; }
}

public class ArmGenerator
{
  public readonly List<string> instructions = new List<string>();

  private readonly StandardLibrary stdlib = new StandardLibrary();

  private List<StackObject> stack = new List<StackObject>();
  private int depth = 0;
public int CurrentDepth => depth;

  public int labelCounter = 0;

  public string GetLabel()
  {
    return $"L{labelCounter++}";
  }

  public void SetLabel(string label)
  {
    instructions.Add($"{label}:");
  }

  //stack operations  

  public void PushObject(StackObject obj)
  {
    stack.Add(obj);

  }
  
  public StackObject TopObject()
  {
    return stack.Last();
  }
  public void PushConstant(StackObject obj, object value)
  {
  
  switch (obj.Type)
  {
    case StackObject.StackObjectType.Int:
        Mov(Register.X0 , (int)value);
        Push(Register.X0);

      break;
  case StackObject.StackObjectType.Bool:
      Mov(Register.X0, (bool)value ? 1 : 0);
      Push(Register.X0);

      break;

  case StackObject.StackObjectType.Float:

      long floatBits = BitConverter.DoubleToInt64Bits((double)value);
      short[] floatParts = new short[4];

      for (int i = 0; i < 4; i++)
      {
          floatParts[i] = (short)((floatBits >> (i * 16)) & 0xFFFF);
      }

      instructions.Add($"MOVZ X0, {floatParts[0]}, LSL #0");
      for (int i = 1; i < 4; i++)
      {
          instructions.Add($"MOVK X0, #{floatParts[i]}, LSL #{i * 16}");
      }

      Push(Register.X0);
      break;

    case StackObject.StackObjectType.String:
      // to do
      List<byte> stringArray = Utils.StringTo1Byte((string)value);

      Push(Register.HP);
      for (int i = 0; i < stringArray.Count; i++)
      {
        var charCode = stringArray[i];
        Comment($"Pushing character {charCode} to heap");

        Mov("w0", charCode);
        Strb("w0", Register.HP);
        Mov(Register.X0, 1);
        Add(Register.HP, Register.HP, Register.X0);
      }
      break;

  }

  PushObject(obj);

  }

public void PrintFloat()
{
    stdlib.Use("print_integer"); 
    stdlib.Use("print_double");
    
    instructions.Add("BL print_double");
}

public StackObject PopObject(string rd)
{


    var obj = stack.Last();
    stack.RemoveAt(stack.Count - 1);

    Pop(rd);
    return obj;
}   

  public StackObject IntObject()
  {


    return new StackObject
    {
      Type = StackObject.StackObjectType.Int,
      Length = 8,
      Depth = depth,
      Id = null
    };

  }
   public StackObject BoolObject()
  {


    return new StackObject
    {
      Type = StackObject.StackObjectType.Bool,
      Length = 8,
      Depth = depth,
      Id = null
    };

  }



  public StackObject FloatObject()
  {
    return new StackObject
    {
      Type = StackObject.StackObjectType.Float,
      Length = 8,
      Depth = depth,
      Id = null
    };
  }

  public StackObject StringObject()
  {
    return new StackObject
    {
      Type = StackObject.StackObjectType.String,
      Length = 8,
      Depth = depth,
      Id = null
    };
  }

  public StackObject CloneObject(StackObject obj)
  {
    return new StackObject
    {
      Type = obj.Type,
      Length = obj.Length,
      Depth = obj.Depth,
      Id = obj.Id
    };

  }

  //environment operations

  public void NewScope()
  {
    depth++;
  }

public int EndScope()
{
    int byteOffset = 0;

    for (int i = stack.Count - 1; i >= 0; i--)
    {
        if (stack[i].Depth == depth)
        {
            byteOffset += stack[i].Length;
            stack.RemoveAt(i);
        }
        else
        {
            break;
        }
    }
    depth--;

    return byteOffset;
}
  public void TagObject(string id)
  {
    stack.Last().Id = id;
  }

public (int, StackObject) GetObject(string id)
{
    int byteOffset = 0;
    for (int i = stack.Count - 1; i >= 0; i--)
    {
        if (stack[i].Id == id)
        {
            return (byteOffset, stack[i]); // Devuelve el offset y el objeto encontrado
        }
        byteOffset += stack[i].Length; // Incrementa el offset según el tamaño del objeto
    }
    throw new Exception($"Object {id} not found in stack");
}

  public void Add(string rd, string rs1, string rs2)
  {
    instructions.Add($"ADD {rd}, {rs1}, {rs2}");
  }
  public void Sub(string rd, string rs1, string rs2)
  {
    instructions.Add($"SUB {rd}, {rs1}, {rs2}");
  }
  public void Mul(string rd, string rs1, string rs2)
  {
    instructions.Add($"MUL {rd}, {rs1}, {rs2}");
  }
  public void Div(string rd, string rs1, string rs2)
  {
    instructions.Add($"SDIV {rd}, {rs1}, {rs2}");
  }

    public void Addi(string rd, string rs1, int imm)
    {
        instructions.Add($"ADDI {rd}, {rs1}, #{imm}");
    }

    // -memory opertions
    public void Str(string rs1, string rs2, int offset = 0)
    {
        instructions.Add($"STR {rs1}, [{rs2}, #{offset}]");
    }

    public void Strb(string rs1, string rs2)
    {
        instructions.Add($"STRB {rs1}, [{rs2}]");
    }

    public void Ldr(string rd, string rs1, int offset = 0)
    {
        instructions.Add($"LDR {rd}, [{rs1}, #{offset}]");
    }

    public void Mov(string rd, int imm)
    {
        instructions.Add($"MOV {rd}, #{imm}");
    }

// Mov entre registros
public void Mov(string rd, string rs)
{
    instructions.Add($"MOV {rd}, {rs}");
}


    public void Push(string rs)
    {
        instructions.Add($"STR {rs}, [SP, #-8]!"); // Push to stack
    }
    
    public void Pop(string rd)
    {
        instructions.Add($"LDR {rd}, [SP], #8"); // Pop from stack
    }

    public void Svc()
    {
        instructions.Add($"SVC #0");
    }

    //  Convierte entero con signo (Xn) a flotante (Dn)
      public void Scvtf(string fd, string xn)
      {
          instructions.Add($"SCVTF {fd}, {xn}");
      }

      //  Convierte flotante (Dn) a entero con truncamiento (Xn)
      public void Fcvtzs(string xn, string fd)
      {
          instructions.Add($"FCVTZS {xn}, {fd}");
      }

      // Suma de flotantes
      public void Fadd(string rd, string rs1, string rs2)
      {
          instructions.Add($"FADD {rd}, {rs1}, {rs2}");
      }

      //  Resta de flotantes
      public void Fsub(string rd, string rs1, string rs2)
      {
          instructions.Add($"FSUB {rd}, {rs1}, {rs2}");
      }

      // Multiplicación de flotantes
      public void Fmul(string rd, string rs1, string rs2)
      {
          instructions.Add($"FMUL {rd}, {rs1}, {rs2}");
      }

      // División de flotantes
      public void Fdiv(string rd, string rs1, string rs2)
      {
          instructions.Add($"FDIV {rd}, {rs1}, {rs2}");
      }


    public void Fmov(string rd, string rs)
    {
        instructions.Add($"FMOV {rd}, {rs}");
    }


    public void Endprogram()
    {
      Mov(Register.X0, 0); // Set exit code to 0
      Mov(Register.X8, 93); // syscall number for exit
      Svc(); // make syscall
    }

    public void PrintInteger(string rs)
    {
        stdlib.Use("print_integer"); // Use the print_integer function from the standard library
        instructions.Add($"MOV X0, {rs}"); // Load the integer to print
        instructions.Add($"BL print_integer"); 
    }

    public void PrintString(string rs)
    {
        stdlib.Use("print_string"); // Use the print_string function from the standard library
        instructions.Add($"MOV X0, {rs}"); // Load the string to print
        instructions.Add($"BL print_string"); 
    }

    public void Comment(string comment)
    {
        instructions.Add($"// {comment}");
    }
    
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine(".data");
        sb.AppendLine("heap: .space 4096"); // Reserve 4096 bytes for heap 
        sb.AppendLine(".text");
        sb.AppendLine(".global _start");
        sb.AppendLine("_start:");
        sb.AppendLine("    adr x10, heap"); // Load address of heap into x10
         


        Endprogram(); // Call Endprogram at the end of the program
        foreach (var instruction in instructions)
        {
            sb.AppendLine(instruction);
        }
        
        sb.AppendLine("\n\n\n // Standard Library Functions");
        sb.AppendLine(stdlib.GetFunctionDefinitions()); // Add standard library functions at the end
        return sb.ToString();
    }


  //relacionales
public void Cmp(string rs1, string rs2)
{
    instructions.Add($"CMP {rs1}, {rs2}");
}

public void Fcmp(string rd1, string rd2)
{
    instructions.Add($"FCMP {rd1}, {rd2}");
}

public void Cset(string rd, string condition)
{
    instructions.Add($"CSET {rd}, {condition}");
}


//if

 public void Cbz(string rs, string label)
{
    instructions.Add($"CBZ {rs}, {label}"); 
}

public void B(string label)
{
    instructions.Add($"B {label}"); 
}

//comparaciones
public void Beq(string label)
{
    instructions.Add($"BEQ {label}"); 
}
public void Bne(string label)
{
    instructions.Add($"BNE {label}"); 
}
public void Blt(string label)
{
    instructions.Add($"BLT {label}"); 
}
public void Bge(string label)
{
    instructions.Add($"BGE {label}"); 
}
public void Ble(string label)
{
    instructions.Add($"BLE {label}"); 
}


public void Bgt(string label)
{
    instructions.Add($"BGT {label}"); 
}
public void Ret()
{
    instructions.Add($"RET"); 
}
}
