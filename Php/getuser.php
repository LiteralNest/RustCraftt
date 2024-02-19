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

$namecheckquery = "SELECT name, id FROM Users WHERE name ='".$name."';";
$namecheck = mysqli_query($connection, $namecheckquery) or die("Name check query failed");

if(mysqli_num_rows($namecheck) > 0){
    while($row = mysqli_fetch_assoc($namecheck))
    {

        $data[] = array(
        'name' => $row['name'],
        'id' => $row['id']  
        );	
       
        $json_data = json_encode($data);
    	header('Content-Type: application/json');
    	echo $json_data;
		exit();
    }
}

echo "false";

?>