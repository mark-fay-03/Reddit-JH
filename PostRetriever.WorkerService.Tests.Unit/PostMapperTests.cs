using PostRetriever.WorkerService.Services;
using RedditSharp.Things;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Reddit.Data.Contracts;
using RedditSharp;

namespace PostRetriever.WorkerService.Tests.Unit;

public class PostMapperTests
{
    private const string POST_OBJECT_JSON_STRING = "{\"kind\": \"t5\",\"data\": { \"user_flair_background_color\": null, \"submit_text_html\": null, \"restrict_posting\": true, \"user_is_banned\": false, \"free_form_reports\": false, \"wiki_enabled\": null, \"user_is_muted\": false, \"user_can_flair_in_sr\": null, \"display_name\": \"temp2\", \"header_img\": null, \"title\": \"temp2\", \"allow_galleries\": true, \"icon_size\": null, \"primary_color\": \"\", \"active_user_count\": 4, \"icon_img\": \"\", \"display_name_prefixed\": \"r/temp2\", \"accounts_active\": 4, \"public_traffic\": false, \"subscribers\": 1, \"user_flair_richtext\": [], \"videostream_links_count\": 0, \"name\": \"t5_58y2vb\", \"quarantine\": false, \"hide_ads\": false, \"prediction_leaderboard_entry_type\": \"IN_FEED\", \"emojis_enabled\": false, \"advertiser_category\": \"\", \"public_description\": \"\", \"comment_score_hide_mins\": 0, \"allow_predictions\": false, \"user_has_favorited\": false, \"user_flair_template_id\": null, \"community_icon\": \"\", \"banner_background_image\": \"\", \"original_content_tag_enabled\": false, \"community_reviewed\": false, \"submit_text\": \"\", \"description_html\": null, \"spoilers_enabled\": true, \"comment_contribution_settings\": {}, \"allow_talks\": false, \"header_size\": null, \"user_flair_position\": \"right\", \"all_original_content\": false, \"has_menu_widget\": false, \"is_enrolled_in_new_modmail\": null, \"key_color\": \"\", \"can_assign_user_flair\": false, \"created\": 1635348815.0, \"wls\": null, \"show_media_preview\": true, \"submission_type\": \"any\", \"user_is_subscriber\": false, \"allowed_media_in_comments\": [], \"allow_videogifs\": true, \"should_archive_posts\": false, \"user_flair_type\": \"text\", \"allow_polls\": true, \"collapse_deleted_comments\": false, \"emojis_custom_size\": null, \"public_description_html\": null, \"allow_videos\": true, \"is_crosspostable_subreddit\": true, \"notification_level\": null, \"should_show_media_in_comments_setting\": true, \"can_assign_link_flair\": false, \"accounts_active_is_fuzzed\": false, \"allow_prediction_contributors\": false, \"submit_text_label\": \"\", \"link_flair_position\": \"\", \"user_sr_flair_enabled\": true, \"user_flair_enabled_in_sr\": true, \"allow_chat_post_creation\": true, \"allow_discovery\": true, \"accept_followers\": true, \"user_sr_theme_enabled\": true, \"link_flair_enabled\": false, \"disable_contributor_requests\": false, \"subreddit_type\": \"public\", \"suggested_comment_sort\": null, \"banner_img\": \"\", \"user_flair_text\": null, \"banner_background_color\": \"\", \"show_media\": true, \"id\": \"58y2vb\", \"user_is_moderator\": false, \"over18\": false, \"header_title\": \"\", \"description\": \"\", \"is_chat_post_feature_enabled\": true, \"submit_link_label\": \"\", \"user_flair_text_color\": null, \"restrict_commenting\": false, \"user_flair_css_class\": null, \"allow_images\": true, \"lang\": \"en\", \"whitelist_status\": null, \"url\": \"/r/temp2/\", \"created_utc\": 1635348815.0, \"banner_size\": null, \"mobile_banner_image\": \"\", \"user_is_contributor\": false, \"allow_predictions_tournament\": false}\r\n}";
    private readonly Post _redditSharpPost = new (Substitute.For<IWebAgent>(), JToken.Parse(POST_OBJECT_JSON_STRING));

    private readonly IRedditPosts _redditPosts = Substitute.For<IRedditPosts>();

    private IRedditPost _result = null!;


    [Fact]
    [Trait("Category", "UnitTest")]
    public void ShouldMapToDataType()
    {
        // Act
        WhenMapIsCalled();

        // Assert
        ThenRedditPostWasReturned();
    }

    private void ThenRedditPostWasReturned()
    {
        _result.Id.Should().Be(_redditSharpPost.Id);
        _result.AuthorName.Should().Be(_redditSharpPost.AuthorName);
        _result.UpVotes.Should().Be(_redditSharpPost.Upvotes);
        _result.Created.Should().Be(_redditSharpPost.CreatedUTC);
    }

    private void WhenMapIsCalled()
    {
        IMapper<Post, IRedditPost> target = new PostMapper(_redditPosts);
        _result = target.Map(_redditSharpPost);
    }
}