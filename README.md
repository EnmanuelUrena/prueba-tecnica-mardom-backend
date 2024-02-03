
# .NET 8.0 Backend Application

This WebAPI application is developed with .NET8 


## API Reference

#### Get all employee

```http
  GET /api/employee
```

#### Get one employee

```http
  GET /api/employee/find-by-document?document
```

| Parameter | Type     | Description                        |
| :-------- | :------- | :----------------------------------|
| `document`| `string` | **Required**. Document of employee |

#### Get employees without duplicates

```http
  GET /api/employee/without-duplicates
```

#### Get employees with salary range

```http
  GET /api/employee/salary-range
```

| Parameter | Type     | Description                                        |
| :-------- | :------- | :-------------------------------------------------         |
| `min     `| `number` | **Optional**. indicates the minimum salary. **Default**: 0 |
| `max     `| `number` | **Optional**. indicates the maximum salary. **Default**: 100,000 |

#### Get employees with salary increase

```http
  GET /api/employee/salary-increase
```

#### Get gender percentage of all employees

```http
  GET /api/employee/gender-percentage
```

#### Post a new employee

```http
  POST /api/employee
```

#### Delete a new employee

```http
  Delete /api/employee?document
```
| Parameter | Type     | Description                                  |
| :-------- | :------- | :------------------------------------------- |
| `document`| `string` | **Required**. Document of employee to delete |


## Acknowledgements

 - [.NET 8.0](https://learn.microsoft.com/en-us/dotnet/)



## Authors

- [@EnmanuelUrena](https://github.com/EnmanuelUrena/)

