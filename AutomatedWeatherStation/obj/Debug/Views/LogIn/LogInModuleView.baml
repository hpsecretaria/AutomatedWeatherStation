   M S B A M L   `   `   `  ����R  NAutomatedWeatherStation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:AutomatedWeatherStation.HelpersAutomatedWeatherStation.Helpers  5      9    3AutomatedWeatherStation.Views.LogIn.LogInModuleView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Ahelpers-clr-namespace:AutomatedWeatherStation.Helpers     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5        � DataContext  5      ��    Locator �� Source# [   �� Path$ LogInModule�.��5
      � .+�5   
   �� $��0,15,0,0q�6   
������ 
������ $��1q�5       � UseLayoutRounding .  J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         
BlurRadius$ 10Q�6#      	Direction$	 270Q�5   #      ShadowDepth$ 5Q�5   #      Color$ #BBBBBB��5   #   5      �� 5   
   
������ $��1q�5      	 �� CornerRadius$	 3��5       . ._�5      � $	��250��6   $��Center=�6   
 � RowDefinitions
 5      �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5       $
��Auto �6$   .+�5"      � $��10,20,10,10q�6    � ColumnDefinitions 5#      �� 5$      $
��Auto �6+   �� 5%      $��* �6+   
 5'      �� 5(      $
��Auto �6(   �� 5)      $
��Auto �6(   .+�5+      �� $��0��6    $��0,0,0,2q�5,       $��Center=�5-       $��Bottom=�5.       $��16(�5/       $
��Bold"�50         AccentColorBrush#���  51       $��USERNAME��52       �� 53      $��0��6   $��1��54      $��10,5,0,0q�55      $��Center=�56      $��Bottom=�57      $��12(�58      $��Medium"�59      ��5:      �� $ UserName��� 5;      $��1��6    $��0��5<       $��0,0,0,2q�5=       $��Center=�5>       $��Bottom=�5?       $��16(�5@       $
��Bold"�5A       #���  5B       $��PASSWORD��5C       G� 5D      $��1��6"   $��1��5E   "   $��10,5,0,0q�5F   "   $��Center=�5G   "   $��Bottom=�5H   "   4   .AutomatedWeatherStation.Helpers.PasswordHelper   Attach .    Password 5J   "   �� $ Password� �� Mode$ TwoWay=�5L      �� 5M      $��0��6   $��3��5N      $��0,15,0,0q�5O      #���  5P      $��1q�5Q      $	 3��5R      � 5S      $��1��6   $��	10,5,10,5q�5T      y�5U      �� # [  $ LogInModule.Error�  EmptyStringToVisConverter �� 	Converter# [  �� UpdateSourceTrigger$ PropertyChanged=�.+�5Y      � y�5Z      �� # [  $ LogInModule.IsLoggingIn�  BoolToVisConverter# [ $ PropertyChanged=�.+�5[      �� $��Center=�6$   $	��Top=�5\   $   $��12(�5]   $   $
��Thin"�5^   $   
���  ����5`   $   �� $ Error�5a      5b      � 5c      $��1��6   $��	10,5,10,5q�5d      $	��Top=�5e       � IsIndeterminate . y�5g      �� # [  $ LogInModule.IsLoggingIn� �� ConverterParameter$ inverse��# [ $ PropertyChanged=��� 5l      $��2��6   $��
10,0,10,10q�6*   .+�5m      �� $��LOGIN��6C   ��6   �� $ LogInCommand�6C   5n      5o      5p      5q      5r      