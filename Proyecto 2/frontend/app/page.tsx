'use client';
import Editor from '@monaco-editor/react';
import { useState, useRef } from 'react';

const API_URL = 'http://localhost:5142';

type Symbol = {
  id: string;
  typeSymbol: string;
  dataType: string;
  scope: string;
  line: number;
  column: number;
};

interface Error {
  type: string;
  message: string;
  line: number;
  column: number;
}

export default function Home() {
  const [code, setCode] = useState<string>('');
  const [output, setOutput] = useState<string>('Esperando ejecución...');
  const [error, setError] = useState<string>('');
  const [symbolTable, setSymbolTable] = useState<Symbol[]>([]);
  const [errors, setErrors] = useState<Error[]>([]);
  const [showErrors, setShowErrors] = useState<boolean>(false);
  const [astImageUrl, setAstImageUrl] = useState<string | null>(null);

  const fetchAPI = async (endpoint: string, method: "GET" | "POST") => {
    setError('');
    console.log(`Enviando solicitud (${method}) a ${API_URL}/${endpoint}...`);

    try {
      const response = await fetch(`${API_URL}/${endpoint}`, {
        method,
        headers: { 'Content-Type': 'application/json' },
        ...(method === "POST" && { body: JSON.stringify({ code }) }) 
      });

      const text = await response.text();
      console.log(` Respuesta del servidor (${endpoint}):`, text);

      if (!text.trim()) throw new Error("Error: Respuesta vacía del servidor");

      const data = JSON.parse(text);
      if (!response.ok) throw new Error(data.error || "Error desconocido");

      return data;
    } catch (err) {
      console.error(` Error en ${endpoint}:`, err);
      setError(err instanceof Error ? err.message : "Error desconocido");
      return null;
    }
  };

  const handleExecute = async () => {
    console.log(" Ejecutando código...");
    const data = await fetchAPI("compile/execute", "POST");
    if (data) {
      setOutput(data.result || data.output || "⚠ No hay salida");
      setErrors(data.errors || []);
      setShowErrors(false); 
    }
  };

  const handleGetSymbolTable = async () => {
    console.log(" Obteniendo tabla de símbolos...");
    const data = await fetchAPI("compile/symbols", "GET");
    if (data) setSymbolTable(data.symbols || []);
  };

  const handleGetErrors = async () => {
    console.log("Obteniendo reporte de errores...");
    const data = await fetchAPI("compile/errors", "GET");
    if (data) {
      setErrors(data.errors || []);
      setShowErrors(true); //mostrar tabla
      setError(''); 
    }
  };



  const handleGetASTImage = async () => {
    console.log(" Generando AST...");
    const response = await fetch(`${API_URL}/compile/ast-image`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ code }),
    });
  
    const data = await response.json();
  
    if (response.ok) {
      console.log("Imagen generada en:", data.imageUrl);
      setAstImageUrl(`${API_URL}${data.imageUrl}`);
    } else {
      console.error(" Error generando AST:", data.error);
      alert("Error generando AST: " + data.error);
    }
  };
    

  return (
    <div className="flex flex-col items-center justify-center min-h-screen py-2 bg-gray-900 text-white">
      <div className="flex gap-4 mb-4">
        <button className="bg-gray-700 hover:bg-gray-600 text-white font-bold py-2 px-4 rounded"onClick={handleGetASTImage}
        >Reporte AST</button>
        <button className="bg-gray-700 hover:bg-gray-600 text-white font-bold py-2 px-4 rounded" onClick={handleGetSymbolTable}>
          Tabla de Símbolos
        </button>
        <button 
          className={`font-bold py-2 px-4 rounded transition-colors ${
            showErrors ? "bg-red-600 hover:bg-red-500" : "bg-gray-700 hover:bg-gray-600"
          } text-white`}
          onClick={() => setShowErrors(!showErrors)}
        >
          {showErrors ? "Ocultar Errores" : "Reporte Errores"}
        </button>
      </div>

      <Editor height="70vh" defaultLanguage="go" theme="vs-dark" value={code} onChange={(value) => setCode(value || '')} />

      <button className="bg-blue-600 hover:bg-blue-500 text-white font-bold py-2 px-4 rounded mt-4" onClick={handleExecute}>
        Ejecutar
      </button>

      {/* mostrando los errores */}
      {error && !showErrors && (
        <div className="bg-red-600 text-white p-2 rounded mb-4 w-full max-w-3xl text-center">
          <strong>Error:</strong> {error}
        </div>
      )}

      <div className="mt-4 w-full max-w-3xl bg-gray-800 text-white p-4 rounded-md overflow-auto" style={{ height: '200px' }}>
        <h2 className="text-lg font-bold">Consola</h2>
        <pre className="whitespace-pre-wrap">{output}</pre>
      </div>

      {symbolTable.length > 0 && (
        <div className="mt-4 w-full max-w-4xl bg-gray-800 text-white p-4 rounded-md overflow-auto">
          <h2 className="text-lg font-bold">Tabla de Símbolos</h2>
          <table className="w-full border-collapse border border-gray-600 mt-2">
            <thead>
              <tr className="bg-gray-700">
                <th className="border border-gray-600 p-2">ID</th>
                <th className="border border-gray-600 p-2">Tipo</th>
                <th className="border border-gray-600 p-2">Dato</th>
                <th className="border border-gray-600 p-2">Ámbito</th>
                <th className="border border-gray-600 p-2">Línea</th>
                <th className="border border-gray-600 p-2">Columna</th>
              </tr>
            </thead>
            <tbody>
              {symbolTable.map((symbol, index) => (
                <tr key={index} className="bg-gray-900">
                  <td className="border border-gray-600 p-2">{symbol.id}</td>
                  <td className="border border-gray-600 p-2">{symbol.typeSymbol}</td>
                  <td className="border border-gray-600 p-2">{symbol.dataType}</td>
                  <td className="border border-gray-600 p-2">{symbol.scope}</td>
                  <td className="border border-gray-600 p-2">{symbol.line}</td>
                  <td className="border border-gray-600 p-2">{symbol.column}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {showErrors && errors.length > 0 && (
        <div className="mt-4 w-full max-w-4xl bg-red-800 text-white p-4 rounded-md overflow-auto">
          <h2 className="text-lg font-bold">Errores</h2>
          <table className="w-full border-collapse border border-gray-600 mt-2">
            <thead>
              <tr className="bg-red-700">
                <th className="border border-gray-600 p-2">Tipo</th>
                <th className="border border-gray-600 p-2">Mensaje</th>
                <th className="border border-gray-600 p-2">Línea</th>
                <th className="border border-gray-600 p-2">Columna</th>
              </tr>
            </thead>
            <tbody>
              {errors.map((error, index) => (
                <tr key={index} className="bg-red-900">
                  <td className="border border-gray-600 p-2">{error.type}</td>
                  <td className="border border-gray-600 p-2">{error.message}</td>
                  <td className="border border-gray-600 p-2">{error.line}</td>
                  <td className="border border-gray-600 p-2">{error.column}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}