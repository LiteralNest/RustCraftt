<?php 

$servername = "localhost";
$username = "u237911781_root";
$password = "A2rwedf2tre";
$dbName = "u237911781_main";

$connection = new mysqli($severname, $username, $password, $dbName);
if ( ! $connection ) {
   die( 'Could not connect: ' . mysqli_error($con) );
}

$name = $_POST["name"];
$password = $_POST["password"];

$namecheckquery = "SELECT name FROM Users WHERE name ='".$name."';";
$namecheck = mysqli_query($connection, $namecheckquery) or die("Name check query failed");

if(mysqli_num_rows($namecheck) > 0){
	echo " Name already exists ";
	exit();
}

$insertuserquery = "INSERT INTO Users (name, password) VALUES ('".$name."','".$password."');";

if ($connection->query($insertuserquery) === TRUE) {
  echo "true";
} else {
  echo "Error: " . $insertuserquery . "<br>" . $connection->error;
}

?>