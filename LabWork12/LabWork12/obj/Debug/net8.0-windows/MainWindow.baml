   M S B A M L   `   `   `  ����D  @LabWork12, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:LabWork12	LabWork12  5   	       LabWork12.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:LabWork12     5   	     � Title$  
MainWindow��5      $	��500��6    $	��600��6-   .��5
      �  � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $��* �6   .+�5   
   �� $7��1Введите часть имени файла: ��6   
�������66   �� 5   
   -   5         ��Name$ txtFileName��5      $	��300��6'   $��25��63   $��Right=�6?   �� 5   
   -   5         ��Name$ btnSelectFolder��5      $��Выбрать папку��6*   $	��120��6B   $��30��6N   
�������$��2��5      6   �� 5   
   -   5         ��Name$ txtFolderPath��5      $$��Текущая папка: ...��6+   $��1��5      �� 5   
    �� Orientation$ 
Horizontal=�6   $��3��6.   .+�5      	� -   5         	�Name$ rbCurrentFolder��5      $ ��Только в папке��63    d� 	IsChecked$
 TrueY�6L   	� 5      -   5        $ rbIncludeSubfolders��5      $'��!Включая вложенные��67   5      �� 5!   
   $ 
Horizontal=�6   $��4��6.   .+�5"      �� -   5         ��Name$ chkSizeFilter��5"      $0��*Учитывать размер файла��6.   6o   �� 5#      -   5        $ 
txtMinSize��5#      $��50��6*   ${�False��65   �� 5$      $��КБ - ��6   �� 5%      -   5        $ 
txtMaxSize��5%      $��50��6*   ${�False��65   �� 5&      $��КБ - ��6   5'      �� 5)   
   $ 
Horizontal=�6   $��5��6.   .+�5*      �� -	   5        $ chkDateFilter��5*      $2��,Учитывать дату создания��6.   6p   (  "System.Windows.Controls.DatePicker  5+      -
   5        	  Name$	 dtpDateFilter��5+      5,      �� 5.   
   -   5        $ 	btnSearch��5.      $��
Найти��6$   $	��120��64   $��30��6@   $��6��5/      
�������62   �� 51   
   -   5        
 ��Name$
 
lstResults��51      $��7��6&   52      53      