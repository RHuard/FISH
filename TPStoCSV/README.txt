TPS to CSV Automated TOOL v 1.1.0
	Ryan Huard
	
Intended Purpose:
	The purpose of this program is to automatically convert the .TPS files containing
	the mapped points for the fish pictures into a .CSV file.
__________________________________________________________________________________________
Starting the Program:
	Once you have all of the points mapped and saved in their corresponding .TPS files,
	copy this program into the same folder as the .TPS files, and double click the
	application, a console window will open up and the program will run.
__________________________________________________________________________________________	
Running the Program:
	When you are running the program, it will first prompt you for the number of points
	mapped in the TPS files. Type in this number and press the "Enter" key. The default
	value is 15, so if you want to use 15 points, you may just press the "Enter" key.
	
	Next, you will be prompted for an output name. This will be the name of the final
	cvs file. For example, if you wanted to save the data in a file name foo.csv, you 
	would type in "foo" and press the "Enter" key. The ".csv" is automatically added to
	the name, and typing that as part of the name may result in a corrupted file. 
	
	Please note: if a file of the name you entered does not exist, the program 
	will create that file. However, if it does exist, that file will be overwritten.
	
	Now the program will read in the data from the .TPS files and will write to the
	.CSV file. You will see several lines of output. These will tell you what actions
	the program is currently doing and if there were any errors, if there are errors
	the file name and reason for the error will be written to the screen.
	
	Once the program is done writing to the .CSV file, it will check for files with 
	scales outside the standard deviation. You will be prompted for the number of
	standard deviations you want to use as the limit. The default is 3. Enter a number
	just like you did with the number of points and press "Enter" or just press
	"Enter" to use the default value.
	
	After choosing the number of standard deviations to check for, the program will
	compute and output the mean and standard deviation as well as print out any names
	of files that were outside of this range.
	
	Press any key to close the program.
__________________________________________________________________________________________
Update History:
	*added better error handling and better error messages
	*modified output text

__________________________________________________________________________________________
Planned Improvements:
	*graphical interface that is not in the console.
__________________________________________________________________________________________
Contact Me:
	If you have any questions, suggestions for new features or bugs, please contact me
	at: ryan.huard@email.wsu.edu