# TektonAPI
Implementación del challenge solicitado por Tekton creando un rest API en .Net Core (última versión), cuya documentación se realizó usando swagger.

- Se implementó el patrón Repository y Singleton para accesar a la Base de Datos y para implementar filtros de servicios en los endpoint para registar los tiempos de ejecución
- Se aplicaron principios SOLID y Clean Code para el diseño y creación de las clases así como para el nombramiento de variables, clases, etc.
- Se utilizó DataAnnotations y FluentValidation para realizar las validaciones de los datos que llegan a cada Request de los endpoint
- Se implemantaron diferentes HTTP Status Codes en las respuestas de cada EndPoint, asegurando que describan el resultado obtenido
- Se estructuró el proyecto por capas (Datos, negocio y presentación (API Controllers))
- Se sube el poyecto a GitHub en repositorio público

 Se crea este archivo README.md con las siguientes instrucciones para levantar el proyecto localmente, así:

