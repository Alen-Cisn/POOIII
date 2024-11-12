use std::time::Instant;
use tonic::Request;
use user::{user_service_client::UserServiceClient, CreateUserRequest, GetUserRequest};

pub mod user {
    tonic::include_proto!("user");
}

#[tokio::main]
async fn main() -> Result<(), Box<dyn std::error::Error>> {
    let mut client = UserServiceClient::connect("http://169.254.247.69:50051").await?;
    let args: Vec<String> = std::env::args().collect();
    if args.len() > 2 {
        let create_request = Request::new(CreateUserRequest {
            name: args[1].to_string(),
            email: args[2].to_string(),
        });

        let response = client.create_user(create_request).await?;

        println!("user created: {}", response.into_inner().message.to_string());
    } else {

        println!("Empezando 200000 requests POST!");
        let start_create = Instant::now();
        for _ in 1..200000 {
            
            let create_request = Request::new(CreateUserRequest {
                name: "Roberto".to_string(),
                email: "rober.sanchez@uno.edu.ar".to_string(),
            });

            client.create_user(create_request).await?;
        }
        let duration_create = start_create.elapsed();
        println!("Tiempo que tomó la creación de 200000 usuarios: {:?}", duration_create);

        let start_create = Instant::now();
        println!("Empezando 200000 requests GET!");
        let user_id = uuid::Uuid::new_v4().to_string();
        for _ in 1..200000 {
            let get_request = Request::new(GetUserRequest { id: user_id.clone() });
            let _ = client.get_user(get_request.into_inner()).await?;
        }
        let duration_create = start_create.elapsed();
        println!("Tiempo que tomó la lectura de 200000 usuarios: {:?}", duration_create);
    }
    

    Ok(())
}
