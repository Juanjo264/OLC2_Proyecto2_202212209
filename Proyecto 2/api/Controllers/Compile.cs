using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Antlr4.Runtime;

namespace api.Controllers
{
    [Route("compile")]
    public class CompileController : Controller
    {
        private readonly ILogger<CompileController> _logger;

        public CompileController(ILogger<CompileController> logger)
        {
            _logger = logger;
        }

        public class CompileRequest
        {
            [Required]
            public required string code { get; set; }
        }

        private (LanguageParser parser, ParserRuleContext tree, SyntaxErrorListener syntaxErrorListener, LexicalErrorListener lexicalErrorListener) ParseCode(string code)
        {
            var inputStream = new AntlrInputStream(code);
            var lexer = new LanguageLexer(inputStream);
            var lexicalErrorListener = new LexicalErrorListener();
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(lexicalErrorListener);

            var tokenStream = new CommonTokenStream(lexer);
            var parser = new LanguageParser(tokenStream);
            var syntaxErrorListener = new SyntaxErrorListener();
            parser.RemoveErrorListeners();
            parser.AddErrorListener(syntaxErrorListener);

            var tree = parser.program();
            return (parser, tree, syntaxErrorListener, lexicalErrorListener);
        }

        [HttpPost("execute")]
        public IActionResult Post([FromBody] CompileRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(new { error = "Invalid input" });

            try
            {
                CompilerVisitor.GlobalSymbolTable = new SymbolTable();

                var (_, tree, syntaxErrorListener, lexicalErrorListener) = ParseCode(request.code);
                var visitor = new CompilerVisitor();
                visitor.Visit(tree);
                visitor.EjecutarMain(visitor, tree);

                Console.WriteLine(" Salida de ejecución: "+ visitor.output);

                var errors = new List<Error>();
                errors.AddRange(syntaxErrorListener.Errors);
                errors.AddRange(lexicalErrorListener.Errors);

                return Ok(new { result = visitor.output ?? "", errors });
            }
            catch (RecognitionException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (SemanticError e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpGet("symbols")]
        public IActionResult GetSymbolTable()
        {

            try
            {

                Console.WriteLine(" Enviando tabla de símbolos...");

                var predefinedVariables = new HashSet<string> { "time", "strconv.Atoi", "strconv.ParseFloat", "reflect.TypeOf", "slices.Index", "strings.Join", "len", "append" };

                var filteredSymbols = CompilerVisitor.GlobalSymbolTable.Symbols
                    .Where(symbol => !predefinedVariables.Contains(symbol.ID))
                    .ToList();

                foreach (var symbol in filteredSymbols)
                {
                    Console.WriteLine($"ID: {symbol.ID}, Tipo: {symbol.TypeSymbol}, Dato: {symbol.DataType}, Ámbito: {symbol.Scope}, Línea: {symbol.Line}, Columna: {symbol.Column}");
                }


                return Ok(new { symbols = filteredSymbols });
                
                
            }
            catch (Exception e)
            {
                Console.WriteLine(" Error en GetSymbolTable: " + e.Message);
                return BadRequest(new { error = e.Message });
            }
        }

[HttpPost("ast-image")]
public IActionResult GenerateASTImage([FromBody] CompileRequest request)
{
    if (!ModelState.IsValid) return BadRequest(new { error = "Invalid input" });

    try
    {
        var (_, tree, _, _) = ParseCode(request.code);
        string dot = ASTExporter.ConvertToDOT(tree, new LanguageParser(new CommonTokenStream(new LanguageLexer(new AntlrInputStream(request.code)))));

        string backendPath = AppContext.BaseDirectory;
        string dotFile = Path.Combine(backendPath, "ast.dot");
        string outputPng = Path.Combine(backendPath, "ast.png");
        string outputPdf = Path.Combine(backendPath, "ast.pdf");
        System.IO.File.WriteAllText(dotFile, dot);
        Console.WriteLine($" Archivo DOT guardado en: {dotFile}");

        // Generate PDF
        var processPdf = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dot",
                Arguments = $"-Tpdf -Gdpi=150 \"{dotFile}\" -o \"{outputPdf}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        processPdf.Start();
        processPdf.WaitForExit();

        // Generate PNG
        var processPng = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dot",
                Arguments = $"-Tpng -Gdpi=1200 \"{dotFile}\" -o \"{outputPng}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        processPng.Start();
        processPng.WaitForExit();

        if (!System.IO.File.Exists(outputPng) || !System.IO.File.Exists(outputPdf))
        {
            return BadRequest(new { error = "No se pudo generar la imagen del AST." });
        }

        Console.WriteLine($" Imagen del AST guardada en: {outputPng}");
        Console.WriteLine($" PDF del AST guardado en: {outputPdf}");

        return Ok(new { message = "AST generado correctamente." });
    }
    catch (Exception e)
    {
        return BadRequest(new { error = "Error generando AST: " + e.Message });
    }
}    
    
    }
}