# TektonAPI
Implementación del challenge propuesto por Tekton creando un rest API en .Net Core (última versión 8.0), cuya documentación se realizó usando swagger.

- Se implementó el patrón Repository y Singleton para accesar a la Base de Datos y para implementar filtros de servicios en los endpoint para registar los tiempos de ejecución, así como Inyección de Dependencias para los servicios del API
- Se aplicaron principios SOLID y Clean Code para el diseño y creación de las clases así como para el nombramiento de variables, clases, etc.
- Se crearon los 3 endpoints propuestos (GET, POST y PUT) para el maestro de productos y se creó el archivo plano donde queda registrado el tiempo de respuesta de cada request usando IAsyncActionFilter y ILogger estableciendo el ServiceFilter en la defición de cada endpoint
- Se utilizó DataAnnotations y FluentValidation para realizar las validaciones de los datos que llegan a cada Request de los endpoint
- Con LazyCache se implementó el diccionario de estados del producto (refresh cada 5 minutos)
- Para grabar la información del producto localmente se utilizó LocalDB de SQLServer Express ya que requiere un conjunto mínimo de archivos para iniciar el Motor de base de datos de SQL Server en la máquina cliente
- Se implementaron diferentes HTTP Status Codes en las respuestas de cada EndPoint, asegurando que describan el resultado obtenido
- Los porcentaje de descuento se obtienen del siguiente servicio extreno creado --> https://666ee659f1e1da2be5216f0b.mockapi.io/api/tekton/DiscountByProductId/
- Se estructuró el proyecto por capas (Datos, negocio y presentación (API Controllers))
- Se sube el poyecto a GitHub en repositorio público

 Se crea este archivo README.md con las siguientes instrucciones para levantar el proyecto localmente, así:

1) Se descarga e instala LocalDB de SQL Server 2019 Express Edition desde el sgte link
https://download.microsoft.com/download/7/c/1/7c14e92e-bdcb-4f89-b7cf-93543e7112d1/SqlLocalDB.msi

2) Por medio de la línea de comandos (en modo Administrador) se crea una instancia de SQLServer llamada "Tekton" y se inicia (con el comando -s) para utilizarla de inmediato
   
```bash
sqllocaldb create Tekton -s
```
Al ejecutar el comando, debe verse así:

![image](https://github.com/wildergallego/TektonAPI/assets/59023933/063045ee-fe45-408e-b11d-4b956ed93d4b)

3) Luego en Visual Studio 2022, en el "SQL Server Object Explorer" se agrega una nueva conexión a SQL Server, seleccionando la instancia que creamos en el punto anterior llamada "Tekton", así:

![image](https://github.com/wildergallego/TektonAPI/assets/59023933/d7950123-6404-483c-a0ed-3eb4536b8c11)

Luego,

![image](https://github.com/wildergallego/TektonAPI/assets/59023933/1b5a89eb-48bb-4844-b9bd-cc50be34d5c0)

4) Finalmente, debemos hacer el attach de la base de datos de sql server (archivos .mdf y .ldf incluidos en la carpeta DataLayer\LocalDB del proyecto Tekton) a la instancia SQL que acabamos de crear, ejecutando el siguiente comando en una nueva ventana de query de SQLServer en Visual Studio 2022, así:

![image](https://github.com/wildergallego/TektonAPI/assets/59023933/19346fdd-0df9-43ce-af26-eefa56aae15f)

```sql
USE master
CREATE DATABASE ProductsDB
ON (FILENAME = 'G:\TektonAPI\Tekton\DataLayer\LocalDB\ProductsDB.mdf'), --Se debe reemplazar "G:\TektonAPI" por la ruta donde se descargó el proyecto
(FILENAME = 'G:\TektonAPI\Tekton\DataLayer\LocalDB\ProductsDB.ldf') --Se debe reemplazar "G:\TektonAPI" por la ruta donde se descargó el proyecto
FOR ATTACH;
```

5) Luego se hace refresh y ya debe aparecer la base de datos SQL Server local, lista para ser usada por el API

![image](https://github.com/wildergallego/TektonAPI/assets/59023933/8e20ac6b-f957-4773-8e8e-746b5c3e4ef5)

![image](https://github.com/wildergallego/TektonAPI/assets/59023933/2e275752-ba4a-47f5-ab6b-eac8c53712c3)


Tras esta configuración se puede correr la API desde Visual Studio 2022 y se debe ver así en swagger:

![image](https://github.com/wildergallego/TektonAPI/assets/59023933/f48b003a-dcc6-40c7-9332-b4f9b5e78112)


El archivo plano con donde queda registrado el tiempo de respuesta de cada request se genera en la carpeta "AppLog":

![image](https://github.com/wildergallego/TektonAPI/assets/59023933/dedad193-4433-4ee7-a8dc-ce7028657b07)



