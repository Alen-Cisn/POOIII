use std::sync::Arc;

use tonic::{transport::Server, Request, Response, Status};
use user::user_service_server::{UserService, UserServiceServer};
use user::{CreateUserRequest, CreateUserResponse, GetUserRequest, GetUserResponse};
use tokio::sync::Mutex;

pub mod user {
    tonic::include_proto!("user");
}

#[derive(Default)]
pub struct MyUserService {}

pub struct MyUserServiceWrapper(Arc<Mutex<MyUserService>>);

#[tonic::async_trait]
impl UserService for MyUserServiceWrapper {
    async fn create_user(
        &self,
        request: Request<CreateUserRequest>,
    ) -> Result<Response<CreateUserResponse>, Status> {
        let user_id = uuid::Uuid::new_v4().to_string();
        let response = CreateUserResponse {
            id: user_id.clone(),
            message: request.get_ref().email.to_string(),
        };
        Ok(Response::new(response))
    }

    async fn get_user(
        &self,
        request: Request<GetUserRequest>,
    ) -> Result<Response<GetUserResponse>, Status> {
        let response = GetUserResponse {
            id: request.get_ref().id.clone(),
            name: "John Doe".to_string(),
            email: "john.doe@example.com".to_string(),
        };
        Ok(Response::new(response))
    }
}

#[tokio::main]
async fn main() -> Result<(), Box<dyn std::error::Error>> {
    let addr = "0.0.0.0:50051".parse()?;
    
    // Create an Arc<Mutex<MyUserService>> instance
    let user_service = Arc::new(Mutex::new(MyUserService::default()));

    // Wrap the user service in a struct that implements UserService
    let user_service_clone = user_service.clone();
    let service = UserServiceServer::new(MyUserServiceWrapper(user_service_clone));

    Server::builder()
        .http2_keepalive_interval(Some(std::time::Duration::from_secs(30)))
        .max_concurrent_streams(100) // Limit concurrent streams to manage resources
        .add_service(service)
        .serve(addr)
        .await?;

    Ok(())
}