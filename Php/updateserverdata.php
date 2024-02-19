<?php 

$servername = "localhost";
$username = "u237911781_root";
$password = "A2rwedf2tre";
$dbName = "u237911781_main";

$connection = new mysqli($severname, $username, $password, $dbName);
if ( ! $connection ) {
   die( 'Could not connect: ' . mysqli_error($con) );
}

$inputIp = $_POST["ip"];
$inputPlayersCount = $_POST["playersCount"];

$serverCheckQuery = "SELECT ip FROM Servers WHERE ip='" . $inputIp . "';";
$serverCheck = mysqli_query($connection, $serverCheckQuery) or die("Server check query failed");


if(mysqli_num_rows($serverCheck) > 0){
  $updateserverquery = "UPDATE Servers SET playerscount = '" .$inputPlayersCount."' WHERE ip ='" .$inputIp. "';"; 
  $updateserver = mysqli_query($connection, $updateserverquery) or die("Update query failed");
  echo "Updated server query";
  exit();
}

$insertserverquery = "INSERT INTO Servers (ip, playerscount) VALUES ('".$inputIp."','".$inputPlayersCount."');";
$insertserver = mysqli_query($connection, $insertserverquery) or die("Insert query failed");
echo "Insert server query";

?>