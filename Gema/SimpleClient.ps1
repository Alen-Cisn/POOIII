# Define the server and port
$server = "localhost"
$port = 1965  # Replace with your application's port

# Create a TCP client
$tcpClient = New-Object System.Net.Sockets.TcpClient($server, $port)

# Create an SSL stream
$sslStream = New-Object System.Net.Security.SslStream($tcpClient.GetStream(), $false, { $true })
[System.Security.Authentication.SslProtocols]$protocol = "ssl3"
# Load the certificate
$certThumbprint = "82585BFAB0034445E5AE7B6FC76C5D7E0956A2BA"  # Replace with your certificate thumbprint
$cert = Get-ChildItem Cert:\CurrentUser\My | Where-Object { $_.Thumbprint -eq $certThumbprint }
$certcol = New-object System.Security.Cryptography.X509Certificates.X509CertificateCollection
$certcol.Add($cert)

$socket = New-Object Net.Sockets.TcpClient($server, $port)
$stream = $socket.GetStream()
# $sslStream = New-Object System.Net.Security.SslStream $stream,$false
# Authenticate the SSL stream
$sslStream.AuthenticateAsClient($server,$certcol,[System.Security.Authentication.SslProtocols]::Tls13,$false) 
# $sslStream.AuthenticateAsClient($server, $cert, [System.Security.Authentication.SslProtocols]::Tls12, $false)

# Now you can send and receive data
$writer = New-Object System.IO.StreamWriter($sslStream)
$reader = New-Object System.IO.StreamReader($sslStream)

# Example: Send a message
$message = Read-Host "Enter the message to send: "
$writer.WriteLine($message)
$writer.Flush()

# Example: Read a response
$response = $reader.ReadLine()
Write-Host "Received: $response"

# Clean up
$reader.Close()
$writer.Close()
$sslStream.Close()
$tcpClient.Close()