   M S B A M L   `   `   `  ����D  @LabWork22, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:LabWork22	LabWork22  5   	       LabWork22.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:LabWork22     5   	     � Title$  
MainWindow��5   	   $	��472��6   $	��800��6)   .��5	      � .+�5
   
   �� -   5         ��Name$ PasswordListView��5
      $��10,10,10,100q�6-    �� View 5      �� 5      .P�5      �� $#��Сайт/Приложение��6%    �� Width$	 200��6>    �� DisplayMemberBinding 6J   �� *Site+�� 5      $��
Логин��6%   $	 150��64    6@   �� *Login+�� 5      $��Пароль��6%   $	 150��65    6A   �� *DescryptedPassword+5      5      �� 5   
   $��Сайт��6   $��
10,350,0,0q�6   �� 5   
   -   5        $��SiteTextBox��5      $	��180��6%   $��25��61   $��
10,370,0,0q�6=   $
��Left=�6Q   $	��Top=�6l   �� 5   
   $��
Логин��6   $��200,350,0,0q�6    �� 5   
   -   5        $��LoginTextBox��5      $	��140��6&   $��25��62   $��200,370,0,0q�6>   $
��Left=�6S   $	��Top=�6n   �� 5   
   $��Пароль��6   $��350,350,0,0q�6!   �� 5   
   -   5        $��PasswordTextBox��5      $	��180��6)   $��25��65   $��350,370,0,0q�6A   $
��Left=�6V   $	��Top=�6q   �� 5   
   -   5         ��Name$ AddPasswordButton��5   �   $��Добавить��6   $	��100��6$   $��25��60   $��550, 370, 0, 0q�6<   $
��Left=�6T   $	��Top=�6o   6�   �� 5   
   -   5        $��PasswordLengthTextBox��5      $��40��6/   $��25��6:   $��350,400,0,0q�6F   $
��Left=�6[   $	��Top=�6v   $��12��6�   �� 5    
   -   5        $ GeneratePassword��5    �   $-��'Сгенерировать пароль��6   $	��140��60   $��25��6<   $��400,400, 0, 0q�6H   $
��Left=�6_   $	��Top=�6z   6�   5!      5"      