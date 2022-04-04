# BagIt Backend

🛡️ This repository houses the backend for [BagIt](https://github.com/jsam07/bag-it). BagIt a mobile application that improves the grocery shopping experience, by allowing users to easily create and share shopping lists.
## Architecture
The BagIt backend is shown below (highlighted in red). 
<p align="center">
    <img alt="Search" src="./Assets/backend.svg" width="100%"/>
</p>

The backend consists of three(3) main components:

1. **User Authentication API** - Users must first register and login to be able to access the SignalR Hub. 
Clients that are successfully authenticated receive a JSON Web Token (JWT) that can then use to access the Hub.
 The endpoints for the API are as follows:
   ### `GET`
   - `/api/user`: Test that the user endpoint works
   
    ### `POST`
    - `/api/user/register`: Create new user

      - Body:
          - `Username: String` (optional): The username of the user
          - `Email: String` (required): The email address of the user
          - `Password: String` (required): The password of the user
      
    - `/api/user/login`: Sign user in
      - Body:
          - `Email: String` (required): The email address of the user
          - `Password: String` (required): The password of the user

        
2. **SignalR Hub** - The Hub allows for real-time client-to-server (and server-to-client) communication. The former means that users receive shopping list updates in real-time. Unauthorized clients are not able to invoke Hub methods.

3. **SQLite Database** - This self-contained database stores all the list information and users. See the ERD diagram below.

<p align="center">
    <img alt="Search" src="./Assets/ERD_Diagram.png" width="100%"/>
</p>


### Built With

-   ![.NET Maui](https://img.shields.io/badge/-_Maui-050B1E?&logo=dotnet)
-   ![C#](https://img.shields.io/badge/-csharp-050B1E?&logo=csharp)

## Getting Started

### 1. Download starter and install dependencies

Clone this repository:
```
git clone git@github.com:jsam07/bag-it-api.git
```

### 2. Start the REST API server

```
dotnet run
```

The server is now running on ` https://localhost:7210`. You can now make API requests, e.g. [https://localhost:7210/api/user]( https://localhost:7210/api/user).

### 3. Testing SignalR Hub

Recall, you can only access the Hub by providing a valid jwt (generated after successful login). See below for an example:
The jwt token can be obtained by authenticating via the `api/user/login` route specified above - the response shape is as follows:
```json
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjYiLCJleHAiOjE2NDkwNjA4MzksImlzcyI6Imh0dHBzOi8vYmFnLWl0LWFwaS5henVyZXdlYnNpdGVzLm5ldCIsImF1ZCI6Imh0dHBzOi8vYmFnLWl0LWFwaS5henVyZXdlYnNpdGVzLm5ldCJ9.GscRb0fipj79Abl3aUQoT6zUpjxn6oIV0v_SI6t7RME",
    "expiration": "2022-04-04T08:27:19Z"
}
```
Use the `test-client` example project in the root directory to test Hub invocations:
```js
let jwt = "token_here"
const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7210/listHub", { accessTokenFactory: () => jwt })
    .withAutomaticReconnect()
    .build();
```

## Status

<p align="center">
    <img alt="Search" src="https://repobeats.axiom.co/api/embed/91cbe4c222135d9b0057a9dbea404f3bb994a85a.svg" width="100%"/>
</p>