public static class Utils

{
    public static List<byte> StringTo1Byte(string str)
    {
    
    var resultado = new List<byte>();
    int elementIndex = 0;

    while (elementIndex < str.Length)
    {
        resultado.Add((byte)str[elementIndex]);
        elementIndex++;
        
    }
    resultado.Add(0); 
    return resultado;
    }
}