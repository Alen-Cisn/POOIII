use std::time::Instant;
use reqwest::Client;
use serde::{Deserialize, Serialize};
use uuid::Uuid;

#[derive(Serialize)]
struct CreateUserRequest {
    name: String,
    email: String,
}

#[derive(Deserialize)]
struct CreateUserResponse {
    message: String,
}

#[derive(Deserialize)]
struct GetUserRequest {
    id: String,
}

#[derive(Deserialize)]
struct GetUserResponse {
    name: String,
    email: String,
}

#[tokio::main]
async fn main() -> Result<(), Box<dyn std::error::Error>> {
    let client = Client::new();
    let args: Vec<String> = std::env::args().collect();

    if args.len() > 2 {
        let create_request = CreateUserRequest {
            name: args[1].to_string(),
            email: args[2].to_string(),
        };

        let response: CreateUserResponse = client
            .post("http://localhost:8080/users")
            .json(&create_request)
            .send()
            .await?
            .json()
            .await?;

        println!("User created: {}", response.message);
    } else {
        println!("Starting 200,000 POST requests!");
        let start_create = Instant::now();
        for _ in 1..200000 {
            let create_request = CreateUserRequest {
                name: "Roberto".to_string(),
                email: "rober.sanchez@uno.edu.ar".to_string(),
            };

            let a = client
                .post("http://127.0.0.1:8080/users")
                .json(&create_request)
                .send()
                .await?;
        }
        let duration_create = start_create.elapsed();
        println!("Time taken to create 200,000 users: {:?}", duration_create);

        let start_get = Instant::now();
        println!("Starting 200,000 GET requests!");
        let user_id = Uuid::new_v4().to_string();
        for _ in 1..200000 {
            let _ = client
                .get(&format!("http://localhost:8080/users/{}", user_id))
                .send()
                .await?;
        }
        let duration_get = start_get.elapsed();
        println!("Time taken to read 200,000 users: {:?}", duration_get);
    }
    
    Ok(())
}
