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

$serverCheckQuery = "SELECT playerscount FROM Servers WHERE ip='" . $inputIp . "';";
$serverCheck = mysqli_query($connection, $serverCheckQuery) or die("Server check query failed");


if(mysqli_num_rows($serverCheck) > 0){
  $row = mysqli_fetch_assoc($serverCheck);
 echo $row['playerscount'];
 exit();
}

echo '0';

?>