   M S B A M L   `   `   `  ����N  JTrafficViolationApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:TrafficViolationAppTrafficViolationApp  5   	       TrafficViolationApp.Login   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:TrafficViolationApp     5   	     � Title$  Login��5   	    � Icon$ /images/logo.png��5	   	   $	��600��5
   	   $
��1200��5   	    � WindowStartupLocation$ CenterScreen=�5   	   ���#080710.��5      � .+�5   
   �� $��/images/bg.avif��6   $��UniformToFill=�5      $��Stretch=�5      $��Stretch=�5      � 5   
   ���	#55000000$��Center=�5      $��Center=�5      $	��400��5      $	��520��5      $��0,0,0,0q�5      .+�5      �� 
�������$��2q�5      ���	#99000000 �� CornerRadius$ 10��5        �� Padding$ 50,35,50,35q�5!      ._�5"      � $��Center=�6   $��Center=�5#      .+�5$      �� .+�5%      �� $��
Login Here��6$   $��32(�5&   $   $
��Bold"�5'   $   
�������$��Center=�5)   $   $��0,0,0,20q�5*   $   �� 5,      $��Username��6$   
�������$��16(�5.   $   �� 5/      -   5         ��Name$ txtEmail��5/   "   $��40��50   "   $��0,10,0,0q�51   "   $��10q�52   "   ���#333333
�������$��14(�55   "   
�������$��1q�57   "   6S   �� 59      $��Password��6$   
�������$��16(�5;   $   G� 5<      -   5         G�Name$ txtPass��5<   &   $��40��5=   &   $��0,10,0,0q�5>   &   $��10q�5?   &   ���#333333
�������$��14(�5B   &   
�������$��1q�5D   &   6:   �� 5E      -   5         ��Name$ txtError��5E   $   $�� ��66   
���  ��$��16(�5G   $   $��25��5H   $   $��0, 10, 0, 0q�5I   $   �� 5K      -   5         ��Name$ btnLogin��5K   !   $��Log In��5L   !   
����������#080710$��18(�5O   !   $
��Bold"�5P   !   $��50��5Q   !   $��0,5,0,0q�5R   !   
�������$��1q�5T   !   	 �� 	IsDefault$
	 True��6L   �� 5V      
 �� Orientation$
 
Horizontal=�6%   $��Center=�5W   %   $��0,20,0,0q�5X   %   .+�5Y      �� $��Google��6%   ���#DB4437
�������$	��130��5\   %   $��40��5]   %   $��5q�5^   %   
�������$��1q�5`   %   �� 5a      $��Facebook��6%   ���#4267B2
�������$	��130��5d   %   $��40��5e   %   $��5q�5f   %   
�������$��1q�5h   %   5i      5j      5k      5l      5m      5n      5o      