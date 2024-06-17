# TektonAPI
Implementación del challenge propuesto por Tekton creando un rest API en .Net Core (última versión 8.0), cuya documentación se realizó usando swagger.

- Se implementó el patrón Repository y Singleton para accesar a la Base de Datos y para implementar filtros de servicios en los endpoint para registar los tiempos de ejecución, así como Inyección de Dependencias para los servicios del API
- Se aplicaron principios SOLID y Clean Code para el diseño y creación de las clases así como para el nombramiento de variables, clases, etc.
- Se utilizó DataAnnotations y FluentValidation para realizar las validaciones de los datos que llegan a cada Request de los endpoint
- Con LazyCache se implementó el diccionario de estados del producto (refresh cada 5 minutos)
- Para grabar la información del producto localmente se utilizó LocalDB de SQLServer Express ya que requiere un conjunto mínimo de archivos para iniciar el Motor de base de datos de SQL Server en la máquina cliente
- Se implementaron diferentes HTTP Status Codes en las respuestas de cada EndPoint, asegurando que describan el resultado obtenido
- Se estructuró el proyecto por capas (Datos, negocio y presentación (API Controllers))
- Se sube el poyecto a GitHub en repositorio público

 Se crea este archivo README.md con las siguientes instrucciones para levantar el proyecto localmente, así:

1) Se descarga e instala LocalDB de SQL Server 2019 Express Edition desde el sgte link
https://download.microsoft.com/download/7/c/1/7c14e92e-bdcb-4f89-b7cf-93543e7112d1/SqlLocalDB.msi

2) Por medio de la línea de comandos (en modo Administrador) se crea una instancia de SQLServer llamada "Tekton" y se inicia (con el comando -s) para utilizarla de inmediato
   
```bash
sqllocaldb create Tekton -s
```
![image](https://github.com/wildergallego/TektonAPI/assets/59023933/063045ee-fe45-408e-b11d-4b956ed93d4b)


