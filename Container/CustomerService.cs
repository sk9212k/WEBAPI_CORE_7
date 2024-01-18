using AutoMapper;
using LearnAPI.Helper;
using LearnAPI.Modal;
using LearnAPI.Repos;
using LearnAPI.Repos.Models;
using LearnAPI.Service;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

// explanation of the above block
// The code you provided seems to be the beginning of a C# file or namespace. Let's break down the key elements:

//     Using Directives:
//         using AutoMapper;: This directive allows the usage of types and members from the AutoMapper library. AutoMapper is commonly used for object-to-object mapping.
//         using LearnAPI.Helper;: This likely refers to a namespace or class named Helper in the LearnAPI application, providing additional utility functions or classes.
//         using LearnAPI.Modal;: This is likely a namespace or class named Modal in the LearnAPI application, containing data models or structures.
//         using LearnAPI.Repos;: This is likely a namespace or class named Repos in the LearnAPI application, dealing with repositories or data access.
//         using LearnAPI.Repos.Models;: This is likely a namespace or class named Models within the Repos namespace in the LearnAPI application, containing data models related to repositories.
//         using LearnAPI.Service;: This is likely a namespace or class named Service in the LearnAPI application, dealing with services or business logic.
//         using Microsoft.EntityFrameworkCore;: This directive allows the usage of types and members from the Entity Framework Core library. Entity Framework Core is an Object-Relational Mapping (ORM) framework for .NET.

    //static System.Runtime.InteropServices.JavaScript.JSType:
      //  This line imports the JSType enumeration from the System.Runtime.InteropServices.JavaScript namespace. However, this specific namespace and enumeration (JSType) seem unusual and might not be standard in typical C# applications. It's possible that it is specific to a certain environment or framework being used in the application.




namespace LearnAPI.Container
{
    public class CustomerService : ICustomerService
    {
        private readonly LearndataContext context;
        private readonly IMapper mapper;
        private readonly ILogger<CustomerService> logger;
        public CustomerService(LearndataContext context,IMapper mapper,ILogger<CustomerService> logger) { 
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }


// Explanation of the above block of code:

//     Class Definition:
//         public class CustomerService : ICustomerService: Defines a class named CustomerService that implements the ICustomerService interface.

//     Constructor:
//         public CustomerService(LearndataContext context, IMapper mapper, ILogger<CustomerService> logger): Constructor that takes three parameters representing dependencies (context, mapper, and logger). These dependencies are injected into the class during its instantiation.

//     Dependency Injection:
//         LearndataContext context: Represents a database context.
//         IMapper mapper: Represents an object mapper, commonly used for mapping data between different types.
//         ILogger<CustomerService> logger: Represents a logger for capturing log information.

//     Interface Implementation:
        // The class implements the ICustomerService interface. It is implied that the methods declared in the interface (Getall, Getbycode, Remove, Create, Update) need to be implemented in this class.




        public async Task<APIResponse> Create(Customermodal data)
        {
            APIResponse response = new APIResponse();
            try
            {
                this.logger.LogInformation("Create Begins");
                TblCustomer _customer = this.mapper.Map<Customermodal, TblCustomer>(data);
                await this.context.TblCustomers.AddAsync(_customer);
                await this.context.SaveChangesAsync();
                response.ResponseCode = 201;
                response.Result = data.Code;
            }
            catch(Exception ex)
            {
                response.ResponseCode = 400;
                response.Message = ex.Message;
                this.logger.LogError(ex.Message,ex);
            }
            return response;
        }

        public async Task<List<Customermodal>> Getall()
        { 
            List<Customermodal> _response=new List<Customermodal>();
            var _data = await this.context.TblCustomers.ToListAsync();
            if(_data != null )
            {
                _response=this.mapper.Map<List<TblCustomer>,List<Customermodal>>(_data);
            }
            return _response;
        }

        public async Task<Customermodal> Getbycode(string code)
        {
            Customermodal _response = new Customermodal();
            var _data = await this.context.TblCustomers.FindAsync(code);
            if (_data != null)
            {
                _response = this.mapper.Map<TblCustomer, Customermodal>(_data);
            }
            return _response;
        }

        public async Task<APIResponse> Remove(string code)
        {
            APIResponse response = new APIResponse();
            try
            {
                var _customer = await this.context.TblCustomers.FindAsync(code);
                if(_customer != null)
                {
                    this.context.TblCustomers.Remove(_customer);
                    await this.context.SaveChangesAsync();
                    response.ResponseCode = 200;
                    response.Result = code;
                }
                else
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not found";
                }
               
            }
            catch (Exception ex)
            {
                response.ResponseCode = 400;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<APIResponse> Update(Customermodal data, string code)
        {
            APIResponse response = new APIResponse();
            try
            {
                var _customer = await this.context.TblCustomers.FindAsync(code);
                if (_customer != null)
                {
                    _customer.Name = data.Name;
                    _customer.Email = data.Email;
                    _customer.Phone=data.Phone;
                    _customer.IsActive=data.IsActive;
                    _customer.Creditlimit = data.Creditlimit;
                    await this.context.SaveChangesAsync();
                    response.ResponseCode = 200;
                    response.Result = code;
                }
                else
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not found";
                }

            }
            catch (Exception ex)
            {
                response.ResponseCode = 400;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}



// High level Overview:

// This code defines a C# class named CustomerService within the LearnAPI.Container namespace. The purpose of this class seems to be to provide services related to customer data manipulation, such as creation, retrieval, updating, and removal of customer records.

// Let's break down the main components and functionalities of the CustomerService class:

//     Constructor:
//         The class has a constructor that takes three parameters: LearndataContext, IMapper, and ILogger<CustomerService>. These parameters are dependencies injected into the class for database context, object mapping, and logging purposes, respectively.

//     Methods:

//         Create(Customermodal data): This method is used for creating a new customer record. It maps a Customermodal object to a TblCustomer entity using AutoMapper, adds the entity to the database context, and saves the changes to the database. It returns an APIResponse indicating the success or failure of the operation.

//         Getall(): This method retrieves all customer records from the database, maps them to Customermodal objects using AutoMapper, and returns a list of Customermodal objects.

//         Getbycode(string code): Retrieves a customer record from the database based on the provided customer code. It maps the retrieved TblCustomer entity to a Customermodal object and returns it.

//         Remove(string code): Deletes a customer record from the database based on the provided customer code. It returns an APIResponse indicating the success or failure of the operation.

//         Update(Customermodal data, string code): Updates an existing customer record in the database based on the provided customer code. It maps the Customermodal object to the corresponding TblCustomer entity and updates the entity properties. It returns an APIResponse indicating the success or failure of the operation.

//     Exception Handling:
//         The methods include try-catch blocks to handle exceptions. If an exception occurs, it sets the appropriate error code and message in the APIResponse object and logs the exception using the provided logger.

//     Logging:
//         The class utilizes logging to record information and errors during the execution of its methods. It uses an ILogger<CustomerService> for this purpose.

// Overall, this class encapsulates CRUD (Create, Read, Update, Delete) operations for customer data and provides a layer of abstraction for interacting with the underlying database. It leverages AutoMapper for object mapping and logging to capture relevant information during these operations.