   M S B A M L   `   `   `  ����I  EPROG3300Atomic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:PROG3300AtomicPROG3300Atomic  5          PROG3300Atomic.MainWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      .localclr-namespace:PROG3300Atomic     5        � Title$  
MainWindow��5      $	��450��6   $	��800��6&   $��Calibri Light)�62   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $
��1.6* �6   �� 5      $
��0.6* �6   �� 5      $
��1.6* �6   .+�5   
   �� -   5         ��Name$ textBlockDescription1��5       J� Opacity$
 0.85Q�63   
�������$��15q�6U   $��0��6a   $��0��6n   $��Stretch=�6~   $��Stretch=�6�   $��13(�6�   $
��Wrap=�6�   $��CharacterEllipsis=�6�   $:��4Example for displaying information on first chemical��5      �� 5   
   -   5        $ textBlockDescription2��5      $
 0.85Q�63   
�������$��15q�6U   $��2��6a   $��0��6n   $��Stretch=�6~   $��Stretch=�6�   $��13(�6�   $
��Wrap=�6�   $��CharacterEllipsis=�6�   $;��5Example for displaying information on second chemical��5      �� 5   
   -   5         ��Name$ 	imageLeft��5      $��1��6#   $��15q�63   $��Uniform=�6?   �� 5   
   -   5        $ 
imageRight��5      $��2��6$   $��15q�64   $��Uniform=�6@   �� 5   
   -   5        $ textBlockReaction��5      $
 0.85Q�6/   $��3��6>   $��15,15,15,15q�6N   $
��Wrap=�6c   
�������$��0��6�   $��Reaction Description.��5      �� 5   
   -   5        $ imageBottom��5      $��3��6%   $��2��65   $��15q�6B   $��Uniform=�6N   �� 5    
   -   5         ��Name$ 	comboBox1��5       $
 0.85Q�6&   $��1��65   $��2��6E   $��1��6Y   $��Center=�6f   $��Center=�6�   $��0,20,0,0q�6�   $��	25,5,25,5q�6�   �� 5!   
   -   5        $ 	comboBox2��5!      $
 0.85Q�6&   $��1��65   $��2��6E   $��2��6Y   $��Center=�6f   $	��Top=�6�   $��0,20,0,0q�6�   $��	25,5,25,5q�6�   �� 5"   
   $
 0.85Q�6   $��1��6   $��Individual Chemicals��6/   $��2��6N   $��Center=�6b   $	��Top=�6   $��1��6�   $��0,0,0,20q�6�   
�������6�   �� 5$   
   $"��/images/potioncraftmain2.png��6   $��UniformToFill=�66   $��4��6N   $��3��6b    K� ZIndex$ -1��6s    J� Effect 5%      -  'System.Windows.Media.Effects.BlurEffect  5&      	   Radius$	 8Q�6   5(      5)      5*      