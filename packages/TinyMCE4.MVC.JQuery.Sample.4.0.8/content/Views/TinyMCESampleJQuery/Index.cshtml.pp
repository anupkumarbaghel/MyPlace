@model $rootnamespace$.Models.TinyMCEModelJQuery

<h2>Index</h2>

@using (Html.BeginForm()) {

    <fieldset>
        <legend>TinyMCEModel</legend>

        <div class="editor-label">
            @Html.LabelFor(model => model.Basic)
        </div>
        <div class="editor-field">
            @Html.EditorFor(model => model.Basic)
            @Html.ValidationMessageFor(model => model.Basic)
        </div>

		<div class="editor-label">
            @Html.LabelFor(model => model.Classic)
        </div>
        <div class="editor-field">
            @Html.EditorFor(model => model.Classic)
            @Html.ValidationMessageFor(model => model.Classic)
        </div>

		<div class="editor-label">
            @Html.LabelFor(model => model.Full)
        </div>
        <div class="editor-field">
            @Html.EditorFor(model => model.Full)
            @Html.ValidationMessageFor(model => model.Full)
        </div>

        <p>
            <input type="submit" value="Create" />
        </p>
    </fieldset>
}