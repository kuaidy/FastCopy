   M S B A M L   `   `   `  ����C  ?FastCopy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:FastCopy.ViewsFastCopy.Views  5   	       FastCopy.Views.FtpView   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:FastCopy.Views     5   	     � Title$  添加FTP文件夹��5   	   $	��225��6   $	��400��6'    � 
ResizeMode$ NoResize=�63   .��5	      �  � RowDefinitions 5
   
   �� 5      $��30 �6   6*   �� 5      $��30 �6   6*   �� 5      $��30 �6   6*   �� 5      $��30 �6   6*   �� 5      $��30 �6   6*   �� 5      $��30 �6   6*    � ColumnDefinitions 5   
   �� 5      $��80 �6   6,   �� 5      $	��150 �6   6-   �� 5      6!   .+�5   
   �� $��FTP服务器��6   $��Center=�6!   6E   �� 5   
   $��0��6   $��1��6   $��Center=�6/   6S   �� 5   
   $��2��6   $��例如：www.example.com��6    $��Center=�6=   6a   �� 5   
   $��1��6   $��远程文件夹��6   $��Center=�6-   6Q   �� 5   
   $��1��6   $��1��6   $��Center=�6/   6S   �� 5   
   $��1��6   $��2��6   $#��例如：/或者/子文件夹��6-   $��Center=�6C   6g   �� 5   
   $��2��6   $��端口��6   $��Center=�6*   6N   �� 5   
   $��2��6   $��1��6   $��Center=�6/   6S   �� 5   
   $��2��6   $��2��6    $��使用被动模式��60   $��Stretch=�6A   $��Center=�6]   $��0,-1,0,0q�6   $��5,0,0,0q�6�   6�   �� 5    
   $��3��6   $��	用户名��6   $��Center=�6+   6O   �� 5!   
   $��3��6   $��1��6   $��Center=�6/   6S   �� 5"   
   $��4��6   $��密码��6   $��Center=�6*   6N   �� 5#   
   $��4��6   $��1��6   $��Center=�6/   6S   �� 5$   
   $��5��6   $��0��6"   $��3��62    �� Orientation$ 
Horizontal=�6F   $��LeftToRight=�6_   $��Right=�6{   .+�5%      �� $��5��6   $��0��6"   $��80��62   $��25��6=   $��测试连接��6I   $��5,0,0,0q�6X   ��6i   �� *TestConnectCommand+6�   �� 5&      $��5��6   $��1��6"   $��80��62   $��25��6=   $��确定��6I   $��5,0,0,0q�6V   6i   �� 5'      $��5��6   $��2��6"   $��80��62   $��25��6=   $��取消��6I   $��5,0,0,0q�6V   6i   5(      5)      5*      