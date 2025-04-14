using System.Net.NetworkInformation;
using System.Text;
using System.Xml.Serialization;

public class ArmGenerator
{
  private readonly List<string> instructions = new List<string>();

  private readonly StandardLibrary stdlib = new StandardLibrary();

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
    public void Str(string rs1, string rs2, int offset)
    {
        instructions.Add($"STR {rs1}, [{rs2}, #{offset}]");
    }

    public void Ldr(string rd, string rs1, int offset = 0)
    {
        instructions.Add($"LDR {rd}, [{rs1}, #{offset}]");
    }

    public void Mov(string rd, int imm)
    {
        instructions.Add($"MOV {rd}, #{imm}");
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
        instructions.Add($"BL print_integer"); // File descriptor (stdout)
    }

    public void Comment(string comment)
    {
        instructions.Add($"// {comment}");
    }
    
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine(".text");
        sb.AppendLine(".global _start");
        sb.AppendLine("_start:");

        Endprogram(); // Call Endprogram at the end of the program
        foreach (var instruction in instructions)
        {
            sb.AppendLine(instruction);
        }
        
        sb.AppendLine("\n\n\n // Standard Library Functions");
        sb.AppendLine(stdlib.GetFunctionDefinitions()); // Add standard library functions at the end
        return sb.ToString();
    }


}
