Tecnologías y Arquitectura

Para el front se utilizará Angular. 
  
Para el back se utilizará .NET (variables de entorno SSM).
  
Para la arquitectura de la aplicacion se implementará en servicios cloud de AWS. 

![image](https://github.com/user-attachments/assets/9473bfc5-08e1-4103-b92e-a57451da0356)
 

Configuración y Despliegue


•	Backend


Ejecución del CDK cloudformation el cual está contenido en la carpeta del proyecto llamado CDK. 
*      Cdk deploy

  
Para implementar el backend en la Lambda o en el Api Lambda se utiliza el toolkit de visual studio para AWS, el cual se encarga de implementar el .ZIP de la aplicación.

Documentación de Endpoints:

Suscribirse a un Fondo
*		- POST - https://localhost:7048/PensionFund/subscribe-fund

![image](https://github.com/user-attachments/assets/27f5b1b9-0ca8-45e4-926b-a4487bfd4573)

 
Desuscribirse a un Fondo
*		- POST - https://localhost:7048/PensionFund/unsubscribe-fund

![image](https://github.com/user-attachments/assets/0b553e7c-b164-46b1-84e2-643187a2ef21)

 
Obtener una lista de clientes con productos disponibles en una sucursal
*		- POST - https://localhost:7048/PensionFund/get-clients?city=Armenia
Obtener una lista de transacciones realizadas por el cliente
*		- GET - https://localhost:7048/PensionFund/list-transactions?date=2024-10-05
Obtener los productos
*		- GET - https://localhost:7048/PensionFund/get-fundconfiguration


•	Frontend
Ejecución del proyecto frontend para la creación de la carpeta de despliegue.
* 		ng build –prod
Al alojar los archivos del front en el bucket generamos la URL de acceso.
