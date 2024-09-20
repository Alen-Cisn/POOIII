#!/bin/bash

# Define the server and port
server="localhost"
port=1965  # Replace with your application's port

# Load the certificate
cert_path=$(find ~/.local/share/ca-certificates/ -name "*.crt")
key_path=$(find ~/.local/share/ca-certificates/ -name "*.key")

if [ -z "$cert_path" ]; then
    echo "Certificate with thumbprint $cert_thumbprint not found."
    exit 1
fi

# Create an SSL connection and send a message
echo "Enter the message to send: "
read message
{
    echo $message
} | openssl s_client -connect "$server:$port" -cert "$cert_path" -key "$key_path" -CAfile "$cert_path" -quiet -ign_eof -servername "$server"