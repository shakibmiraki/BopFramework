# BopFramework
## Back-End (.NetCore 3.1+): 
- Dynamicly register your dependency with IDependencyRegistrar
- Automapper (classes inherited from IOrderedMapperProfile will automaticly register)
- Configuration to enable and disable redis cache and memory cache
- Authentication middleware(JWT implementation)
- Permission implementation
- Domain-Event publish/subscribe
- Database logging implementation
- FluentValidation for server side input validations
- IStartupTask to start task on project initialization
- Localization
- FluentMigration with data provider manager to implement multiple database easyly (mysql and mssql already have implemented)
- IRoutePublisher to register routes dynamically

## Front-End (Angular 10,AngularMaterial) : 
- Login component
- Register component
- Account-Activation component
- Access-Denied component
- PageNotFound component
- ButtonSpinner component
- Spinner component
- Core module (HttpInterceptor,ErrorInterceptor,LocalizationIntercaptor)
- Notification service
- User service
- Lazy-Loading modules

## Front-End (React) : 
- Login page
- Register page
- Account-Activation page
- PageNotFound component
- Spinner component
- Notification service
- Authentication service
- Multiple theme configuration with use of emotion library

