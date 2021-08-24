# CubesFramework
Cubes Framework provides some modules that help developers dealing with advanced problem like security and hardware accessing and so no ... in an easy way.
## Usage
### Dealing with Security module
* Register a singlton object of **Crypto** class with the prefered encryption alog **SHA256 is the Recommended one** into your ioc contanier.
* Register a singlton object of the **RegistryDataManager** class ,that is used to access the application registry data, into your ioc container.
* Register a transient objec of the **License** class ,that's used to manage a device license, into your ioc container.

#### Developing the Client software
***
* Use **HardwareInfo** class that presents set of methods that return all device hardware data and show them into the activation module in your client app.
* Use **CheckLicense** mehtod from the **License** class to check the entered license
* Use **SaveLicense** method from **License** class to save the checked license in a place in your device.
* **SaveLicense** Method supports two storing methods Store to file and store to the windows registery.

#### Developing the Activator software
***
* Use **GenerateLicense** method from **License** class to generate and retrun the license and pass the required parameters to it
1. serila : Comes from the client app
2. password : set your own password key and save it to use in the activator software
* Send the generated License to the target client to use it
