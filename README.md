# StyleHelper
#### Helper en C# para facilitar la aplicación de estilos en las vistas generadas mediante RAZOR de la siguiente forma:

```C#
<div class="row">
        <div class="col-xs-12">
            @numero.AsCurrency().Build()
            @numero.AsCurrency().Bold().Build()
            @numero.AsCurrency().Bold().Italic().SaveAs("MoneyBoldItalic").Build();
            @numero.AsCurrency().Bold().Italic().AtCenter().Build("p")
            @numero.AsCurrency().Bold().Italic().Underlined().AtLeft().Build("h1")
            @numero.AsCurrency().Small().Italic().Underlined().AtLeft().Build("h2")
            @numero.Apply("MoneyBoldItalic", "h3")
        </div>
</div>
```

```HTML
            <span class="text-right">$ 1.000,00</span>
            <span class="text-right"><strong>$ 1.000,00</strong></span>
            <span class="text-right"><strong><em>$ 1.000,00</em></strong></span>;
            <p class="text-center"><strong><em>$ 1.000,00</em></strong></p>
            <h1 class="text-left"><strong><em><u>$ 1.000,00</u></em></strong></h1>
            <h2 class="text-left"><small><em><u>$ 1.000,00</u></em></small></h2>
            <h3 class="text-right"><strong><em>$ 1.000,00</em></strong></h3>
```
