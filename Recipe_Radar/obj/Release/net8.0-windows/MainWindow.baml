   M S B A M L   `   `   `  ���� G  CRecipe_Radar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Recipe_RadarRecipe_Radar      RecipeRadar.MainWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ,localclr-namespace:Recipe_Radar       � Title$  RecipeRadar��$	��600��$	��800��
������ � Icon ��  �� 	UriSource$ Images/logo.ico?� � 	Resources    ComboBoxIngredient �� 
TargetType �� Property �� Value �� 
TargetType d� 	IsChecked	 �� Path
 �� Mode �� RelativeSource � ColumnDefinitions ��Name �� CornerRadius F�Name �� Triggers P� Property P� Value �� 
TargetName J� IsHitTestVisible �� SelectionBoxItem �� SelectionBoxItemTemplate ��Name �� 
IsReadOnly �� 
IsReadOnly �� IsDropDownOpen � AllowsTransparency J� SnapsToDevicePixels � ActualWidth �� MaxDropDownHeight K� IsItemsHost  �� Triggers! �� 	Resources  ButtonStyle��%\  &	        '�� �    '�� �    &	 F    �� " ��.���  � �� �� " ��.R�� .+�d� $��2��$|�false�� �� $	 IsDropDownOpen�$
 TwoWay=� �� *TemplatedParent+���� .R��  �� $��5* ��� $��* �.+��� $ Border��$��2��$ 5�����	#FFCAEEA5���	#FF2F5318$��1q��� $��0��$ 5,0,0,5��$��1q����	#FFCAEEA5���	#FF2F5318$��0,0,1,0q�F� $ Arrow��$��1��
��� d �$��Center=�$��Center=�"����        � 	=  	= � z     	 P�  �@J�IsMouseOver$
 true��. ��� $ Border�� � �� �	#FF92C46BP�  �@d�	IsChecked$
 trueY�. ��� $ Border�� � �� �	#FF92C46B�� $��ContentSite��$ False��#��z #��z #��z��$��3q��� $ PART_EditableTextBox��$y�Hidden=�# z � $��Popup��$��Bottom=�#��z $
 True��$|�False��$��Slide=�.��� $��DropDown��$
 True��#��z #��z .+��� $ DropDownBorder�����	#FFCAEEA5�� $
 True��.���� $
 True��  �� " ��! ���� '��       " ��.���  �@��CornerRadius$ 5���� " ��! ���� '��       " ��.���  �@��CornerRadius$ 5���� " ��.���  � ��$	 120����  � ��$	 150����  � ��$ 5q���  � �� �	#FF92C46B��  � �� �	#FF2F5318��  � �� �� " ��.R��� $ 5��#��z��#��z��#��z��._��� $��Center=�$��Center=�.���� " �� Padding$" 10q�._�� # � RowDefinitions# �� $��* ��� $
��Auto �.+��� $��0��.+��� -   $ ��Name$$ ButtonsPanel��% �� Orientation$% 
Horizontal=�$��Right=�.+��� -   $��Register��#��[ $��Right=��� -   $��Login��#��[ $��Right=��� -   $$ AccountPanel��$% 
Horizontal=�$��Right=�.+��� -   $��
My Account��#��[ $��Right=��� -   $��Log Out��#��[ $��Right=��� $��Images\logo.png��$	��538��$	��100���� ���#93dd92���#52922a$��2q�$ 10��._��� $��10q�$
}�True��.+��� & �� TextAlignment$
& Left=�$��Center=�$��20(�
���/kU��� $�Ingredients���� -   $ ingredientsTextBox��$��5q����	#FFCAEEA5���	#FF2F5318�� -   ' ��Name$' ingredientListBox��$��5q����	#FFCAEEA5���	#FF2F5318$��65�� ��%5   '��       �� " ��.���  � �� �	#FFC6EAA2���� " ��.���  � y�$ Visible=�  �� ( �� Binding( �� *Items.Count+) �� ElementName$) ingredientListBox��* �� Value* 0.a���  � y�$ 	Collapsed=��� $% 
Horizontal=�$��Center=�.+��� -	   $��Add Ingredient��#��[ �� -
   $��Remove Ingredient��#��[ �� $
& Left=�$��Center=�$��20(�
���/kU��� $�Type of Cuisine���� -   + ��Name$+ CuisinesComboBox��$��5 0 5 10q�#��[  , �� SelectedIndex$, 0������ - �� EndPoint$	- 0,1&�.��� . � Color$. 	#FFF0F0F0��� $. 	#FF3C8026��/ � Offset$/ 1Q�.@��� $�� ���� $��African���� $��American���� $��Asian���� $��European���� $��Middle Eastern���� $
& Left=�$��Center=�$��20(�
���/kU��� $%�Maximum Ready Time (in minutes)���� -   $ timeTextBox��$��5q����	#FFC6EAA2���	#FF2F5318�� $
& Left=�$��Center=�$��20(�
���/kU��� $�Number of Recipes���� -   $+ RecipesComboBox��$��5 0 5 10q�#��[  $, 0������ $	- 0,1&�.��� $. 	#FFF0F0F0��� $. 	#FF3C8026��$/ 1Q�.@��� $��1���� $��2���� $��3���� $��4���� $��5���� $��1��.+�� .+��� -   0 ��Name$0 
FindButton��$��Find Recipes�����	#FF92C46B$��	0,-58,0,0q����	#FF2F5318