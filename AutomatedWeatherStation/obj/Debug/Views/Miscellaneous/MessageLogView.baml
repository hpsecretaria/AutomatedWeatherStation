   M S B A M L   `   `   `  ����  AutomatedWeatherStation@    :AutomatedWeatherStation.Views.Miscellaneous.MessageLogView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5        � DataContext  5      ��    Locator �� Source# [   �� Path$ MiscellaneousModule�.��5      � $��10q�6   .+�5   
   �� $��0,15,0,0q�6   
������ 
������ $��1q�5       � UseLayoutRounding .  J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         
BlurRadius$ 10Q�6#      	Direction$	 270Q�5   #      ShadowDepth$ 5Q�5   #      Color$ #BBBBBB��5   #   5      �� 5   
   
������ $��1q�5       . ._�5      � 	 � RowDefinitions	 5      �� 5      $
��Auto �6$   �� 5      $��* �6$   .+�5!      �� $��0��6   $��2��5"      $��0,15,0,0q�5#        AccentColorBrush#���  5$      $��1q�5%      
 �� CornerRadius$
 3��5&      � 5'      $��0��6   $��0,0,0,5q�6$    � ColumnDefinitions 5(      �� 5)      $
��Auto �6+   �� 5*      $��* �6+   �� 5+      $
��Auto �6+   .+�5.      �� $��10,0,0,0q�6    $��Bottom=�5/         ContextMenuBackgroundBrush#���  50       $��14(�51       $
��Bold"�52       $	��Log��53       �� 54      $��2��6   $��0,0,5,0q�55      $��clear��57      ��56      �� *ClearLogCommand+57      58      �� 59      $��1��6   $��1,1,1,5q�5:      $��0q�5;      $��14(�5<      $��SemiBold"�5=      ��5>      �� $ LogList�5?      5@      5A      5B      