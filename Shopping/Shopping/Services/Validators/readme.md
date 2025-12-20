Validation
Possible implementatation:

- Decorate model with DataAnotation and use ASP .net to validate model
- Implemente validation method in DTO class 
- FluentAssertion - using popular library
- Implementing validation in separate class

Comparring:
- Decorate model with DataAnotation and use ASP .net to validate model
    - fast, simple, and in-box solution (+5 almost no time expenses)
    - does not support async validation (-5 if it requires it does not work at all, so extra validation layer must be implemented)
    - does not support complex logic validation (-5 see previous point)
    - make testign the validation functionality over-complicated (-3 unit test are well appreciated and using integration testing is not good)
- Implemente validation method in DTO class 
    - encupsulate validation logic in one place (2 it is good sometimes, but it does not give a lot profit)
    - make implementation unit test simpler (2 unit test are well appreciated)
    - allow run asunc validation (3 usual requires on third or forth project itteration at least)
    - requires using property DI injection, which is not recommended (0) or implementing extra logic to resolve required services for validation (-1)
    - requires implementation validation logic (-2 it depends on project) or using third party package for this (-1)
- FluentAssertion - using popular library
    - no need to implement custom logic (4 take and use, however you need to declare rules at least)
    - contain reach collection of validations (1 no need to implement custom validation mainly)
    - may become paid for commercial use (-1 may stay free, but in case it starts requiring licencing it requires lots of work to rewrite, depends on project)
    - allow implement unit test in usual way (2 unit test are well appreciated)
- Implementing validation in separate class
    - requires full implementation and support (-4 it depends on skill how it will be implemented)
    - you can implement only what you need (1 some free spaces)
    - allow implement unit test in usual way (2 unit test are well appreciated)
    - it is free it to add anywhere it is needed (2 good to know you can have some base whick can use to make money)

Just quick analyze validation approaches to decide if I need to migrate from FluentValidation now