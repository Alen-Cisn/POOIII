use actix_web::{web, App, HttpServer, HttpResponse, Responder};
use serde::{Deserialize, Serialize};
use std::sync::{Arc, Mutex};
use uuid::Uuid;

#[derive(Default)]
pub struct MyUserService {}

#[derive(Serialize, Deserialize)]
pub struct CreateUserRequest {
    email: String,
}

#[derive(Serialize)]
pub struct CreateUserResponse {
    id: String,
    message: String,
}

#[derive(Serialize, Deserialize)]
pub struct GetUserRequest {
    id: String,
}

#[derive(Serialize)]
pub struct GetUserResponse {
    id: String,
    name: String,
    email: String,
}

impl MyUserService {
    async fn create_user(&self, req: CreateUserRequest) -> CreateUserResponse {
        let user_id = Uuid::new_v4().to_string();
        CreateUserResponse {
            id: user_id.clone(),
            message: req.email,
        }
    }

    async fn get_user(&self, req: GetUserRequest) -> GetUserResponse {
        GetUserResponse {
            id: req.id,
            name: "John Doe".to_string(),
            email: "john.doe@example.com".to_string(),
        }
    }
}

#[actix_web::main]
async fn main() -> std::io::Result<()> {
    let user_service = Arc::new(Mutex::new(MyUserService::default()));

    HttpServer::new(move || {
        let user_service_clone = Arc::clone(&user_service);
        App::new()
            .app_data(web::Data::new(user_service_clone))
            .route("/users", web::post().to(create_user))
            .route("/users/{user_id}", web::get().to(get_user))
    })
    .bind("127.0.0.1:8080")?
    .run()
    .await
}

// Handler for creating a user
async fn create_user(
    user_service: web::Data<Arc<Mutex<MyUserService>>>,
    req: web::Json<CreateUserRequest>,
) -> impl Responder {
    let service = user_service.lock().unwrap();
    let response = service.create_user(req.into_inner()).await;
    HttpResponse::Created().json(response)
}

async fn get_user(
    user_service: web::Data<Arc<Mutex<MyUserService>>>,
    user_id: web::Path<String>, // Change to Path to get user ID from URL
) -> impl Responder {
    let service = user_service.lock().unwrap();
    let request = GetUserRequest { id: user_id.into_inner() }; // Create request from path
    let response = service.get_user(request).await; // Call the service with the request
    HttpResponse::Ok().json(response)

}