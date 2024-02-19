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
$inputpassword = $_POST["password"];

$namecheckquery = "SELECT password FROM Users WHERE name='" . $name . "';";
$namecheck = mysqli_query($connection, $namecheckquery) or die("Name check query failed");
if(mysqli_num_rows($namecheck) != 1){
	echo "Either no user with name, or more that one";
	exit();
}

if(mysqli_fetch_assoc($namecheck)['password'] == $inputpassword){
    echo('true');
    exit();
}

echo('Wrong password!');
?>